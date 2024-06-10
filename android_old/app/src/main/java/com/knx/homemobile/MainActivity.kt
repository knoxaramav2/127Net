package com.knx.homemobile

import android.content.Intent
import android.os.Bundle
import android.util.Log
import android.view.Menu
import android.view.View
import androidx.appcompat.app.AppCompatActivity
import androidx.drawerlayout.widget.DrawerLayout
import androidx.navigation.findNavController
import androidx.navigation.ui.AppBarConfiguration
import androidx.navigation.ui.navigateUp
import androidx.navigation.ui.setupActionBarWithNavController
import androidx.navigation.ui.setupWithNavController
import com.google.android.material.navigation.NavigationView
import com.google.android.material.snackbar.Snackbar
import com.knx.homemobile.databinding.ActivityMainBinding
import com.knx.homemobile.ui.authorization.AuthorizationFragment
import com.knx.homemobile.ui.authorization.AuthorizationPages
import com.knx.netcommons.Data.DbCtx
import com.knx.netcommons.Data.LocalData
import secure.AuthorizeManager

class MainActivity : AppCompatActivity() {

    private lateinit var appBarConfiguration: AppBarConfiguration
    private lateinit var binding: ActivityMainBinding

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        if(REBUILD_ALL){ cleanSlate() }
        else{ DbCtx.getInstance(this) }

        binding = ActivityMainBinding.inflate(layoutInflater)
        setContentView(binding.root)

        setSupportActionBar(binding.appBarMain.toolbar)

        val drawerLayout: DrawerLayout = binding.drawerLayout
        val navView: NavigationView = binding.navView
        val navController = findNavController(R.id.nav_host_fragment_content_main)
        // Passing each menu ID as a set of Ids because each
        // menu should be considered as top level destinations.
        appBarConfiguration = AppBarConfiguration(setOf(
                R.id.nav_home, R.id.nav_apps, R.id.nav_devices, R.id.nav_settings), drawerLayout)
        setupActionBarWithNavController(navController, appBarConfiguration)
        navView.setupWithNavController(navController)
    }

    override fun onResume() {
        enforceLogin()
        super.onResume()
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

    private fun enforceLogin(){
        Log.d("OTS_INFO", "ENFORCE LOGIN")
        localData = LocalData(this)
        if (localData!!.isSetupComplete()) { return  }

        val contextView = findViewById<View>(R.id.nav_host_fragment_content_main)!!
        val authManager= AuthorizeManager(this)
        if(authManager.tryAutoAuthenticate()){
            val user = authManager.getAuthenticatedUser()!!
            Snackbar.make(contextView, "Authenticated as ${user.username}", Snackbar.LENGTH_SHORT).show()
            return
        }

        val loginIntent = Intent(this, AuthorizationActivity::class.java)
        loginIntent.putExtra(AuthorizationFragment.EXTRA_SOLE_PAGE, AuthorizationPages.RegisterPage.value)
        loginIntent.putExtra(AuthorizationFragment.EXTRA_ADMIN_REGISTER, true)
        startActivity(loginIntent)
    }

    override fun onStop() {
        localData?.save()
        super.onStop()
    }

    private fun cleanSlate(){
        DbCtx.getInstance(this, true)
        LocalData(this).resetHard()
    }

    companion object{
        var localData: LocalData? = null
        const val REBUILD_ALL: Boolean = true
    }
}