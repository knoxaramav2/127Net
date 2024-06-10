package com.knx.otsmobile

import android.content.Intent
import android.os.Bundle
import android.util.Log
import android.view.Menu
import android.view.MenuItem
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import androidx.drawerlayout.widget.DrawerLayout
import androidx.navigation.findNavController
import androidx.navigation.ui.AppBarConfiguration
import androidx.navigation.ui.navigateUp
import androidx.navigation.ui.setupActionBarWithNavController
import androidx.navigation.ui.setupWithNavController
import com.google.android.material.navigation.NavigationView
import com.knx.otscommon.auth.AuthManager
import com.knx.otscommon.data.DbCtx
import com.knx.otscommon.data.LocalData
import com.knx.otscommon.util.OTSLog
import com.knx.otsmobile.databinding.ActivityMainBinding
import com.knx.otsmobile.ui.auth.AuthFragment
import com.knx.otsmobile.ui.auth.AuthorizationPages

class MainActivity : AppCompatActivity() {

    private lateinit var appBarConfiguration: AppBarConfiguration
    private lateinit var binding: ActivityMainBinding
    private lateinit var localData: LocalData
    private lateinit var authManager: AuthManager

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        binding = ActivityMainBinding.inflate(layoutInflater)
        setContentView(binding.root)

        setSupportActionBar(binding.appBarMain.toolbar)

        val drawerLayout: DrawerLayout = binding.drawerLayout
        val navView: NavigationView = binding.navView
        val navController = findNavController(R.id.nav_host_fragment_content_main)
        // Passing each menu ID as a set of Ids because each
        // menu should be considered as top level destinations.
        appBarConfiguration = AppBarConfiguration(
            setOf(
                R.id.nav_home, R.id.nav_apps, R.id.nav_devices, R.id.nav_network
            ), drawerLayout
        )
        setupActionBarWithNavController(navController, appBarConfiguration)
        navView.setupWithNavController(navController)
    }

    override fun onCreateOptionsMenu(menu: Menu): Boolean {
        menuInflater.inflate(R.menu.main, menu)
        return true
    }

    override fun onOptionsItemSelected(item: MenuItem): Boolean {
        return when(item.itemId){
            R.id.action_settings ->{
                val intent: Intent = Intent(this, SettingsActivity::class.java)
                startActivity(intent)
                true
            }
            R.id.action_logout ->{
                authManager.logout()
                Toast.makeText(applicationContext, "Logged out", Toast.LENGTH_LONG).show()
                finish()
                startActivity(intent)
                true
            }
            else -> super.onOptionsItemSelected(item)
        }
    }

    override fun onSupportNavigateUp(): Boolean {
        val navController = findNavController(R.id.nav_host_fragment_content_main)
        return navController.navigateUp(appBarConfiguration) || super.onSupportNavigateUp()
    }

    override fun onResume() {
        handleStartupPreRun()
        super.onResume()
    }

    override fun onStop() {
        localData.save()
        super.onStop()
    }

    private fun handleStartupPreRun(){
        authManager = AuthManager.init(this)
        localData = LocalData.init(this)
        OTSLog.logInfo("OTS Pre run")
        handleStartupAuthentication()
    }

    private fun handleStartupAuthentication(){
        val isAuthentic = AuthManager.getInstance().tryAuthenticatePrevious()
        val dao = DbCtx.getInstance(this).getDao()

        if(!isAuthentic) {
            val intent: Intent = Intent(this, AuthActivity::class.java)

            if(!dao.hasUsers()){
                intent.putExtra(AuthFragment.EXTRA_SOLE_PAGE, AuthorizationPages.RegisterPage.value)
            }

            startActivity(intent)
        }

    }
}