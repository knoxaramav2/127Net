package com.knx.homemobile

import android.os.Bundle
import android.util.Log
import android.view.Menu
import androidx.appcompat.app.AppCompatActivity
import androidx.drawerlayout.widget.DrawerLayout
import androidx.navigation.findNavController
import androidx.navigation.ui.AppBarConfiguration
import androidx.navigation.ui.navigateUp
import androidx.navigation.ui.setupActionBarWithNavController
import androidx.navigation.ui.setupWithNavController
import com.google.android.material.navigation.NavigationView
import com.knx.homemobile.databinding.ActivityMainBinding
import com.knx.netcommons.Data.DbCtx
import com.knx.netcommons.Data.Device
import com.knx.netcommons.Data.DeviceOwner
import com.knx.netcommons.Data.LocalData
import com.knx.netcommons.Data.NetMetaData
import com.knx.netcommons.Data.RoleAuthority
import com.knx.netcommons.Data.UserAccount
import java.util.Date

class MainActivity : AppCompatActivity() {

    private lateinit var appBarConfiguration: AppBarConfiguration
    private lateinit var binding: ActivityMainBinding

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        binding = ActivityMainBinding.inflate(layoutInflater)
        setContentView(binding.root)

        systemInits()

        setSupportActionBar(binding.appBarMain.toolbar)

        val drawerLayout: DrawerLayout = binding.drawerLayout
        val navView: NavigationView = binding.navView
        val navController = findNavController(R.id.nav_host_fragment_content_main)
        // Passing each menu ID as a set of Ids because each
        // menu should be considered as top level destinations.
        appBarConfiguration = AppBarConfiguration(setOf(
                R.id.nav_home, R.id.nav_gallery, R.id.nav_slideshow), drawerLayout)
        setupActionBarWithNavController(navController, appBarConfiguration)
        navView.setupWithNavController(navController)
    }

    override fun onCreateOptionsMenu(menu: Menu): Boolean {
        // Inflate the menu; this adds items to the action bar if it is present.
        menuInflater.inflate(R.menu.main, menu)
        return true
    }

    override fun onSupportNavigateUp(): Boolean {
        val navController = findNavController(R.id.nav_host_fragment_content_main)
        return navController.navigateUp(appBarConfiguration) || super.onSupportNavigateUp()
    }

    private fun systemInits(){
        val currDate = Date()
        localData = LocalData()

        try{
            val dao = DbCtx.getInstance(this, true).getDao()
            val metaData = NetMetaData(signInDate = currDate)
            dao.addSignIn(metaData)

            if (!localData!!.isSetupComplete()){

                dao.addRoleAuthority(RoleAuthority(id = 3, roleName = "Guest", authLevel = 2))
                dao.addRoleAuthority(RoleAuthority(id = 2, roleName = "User", authLevel = 1, downgrade = 3))
                dao.addRoleAuthority(RoleAuthority(id = 1, roleName = "Admin", authLevel = 0, downgrade = 2, reauthTime = 60))

                val user = UserAccount(maxAuthority = 1, operatingAuthority = 2)
                val device = Device(address = "", hwId = localData!!.getGuid(),
                    os = localData!!.getOS())

                val deviceId = dao.addDevice(device).toInt()
                val userId = dao.addUserAccount(user).toInt()
                val owner = DeviceOwner(device = deviceId, owner = userId)
                dao.addDeviceOwner(owner)

                localData!!.setSetupComplete()
            }

            Log.d("OTS_INFO", "App start successful")

        } catch (ex: Exception){
            Log.d("OTS_ERR", currDate.toString() + " : " + ex.message.toString())
        }

    }

    companion object{
        var localData: LocalData? = null
    }
}