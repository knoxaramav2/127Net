package com.knx.otsmobile

import android.content.Intent
import android.content.SharedPreferences
import android.os.Bundle
import android.util.Log
import android.view.MenuItem
import androidx.appcompat.app.AppCompatActivity
import androidx.preference.EditTextPreference
import androidx.preference.PreferenceFragmentCompat
import androidx.preference.PreferenceManager
import androidx.preference.SwitchPreference
import androidx.preference.SwitchPreferenceCompat
import com.knx.otscommon.auth.AuthManager
import com.knx.otscommon.data.DbCtx
import com.knx.otscommon.data.LocalData
import com.knx.otscommon.data.OTSDao
import com.knx.otscommon.data.UserAccount
import com.knx.otscommon.data.UserDisplayUpdate
import com.knx.otscommon.data.UserSettings
import com.knx.otscommon.util.OTSLog

class SettingsActivity : AppCompatActivity() {

    lateinit var dao: OTSDao
    lateinit var settings: UserSettings
    lateinit var user: UserAccount
    lateinit var auth: AuthManager
    lateinit var changes: SettingsChanges

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.settings_activity)

        dao = DbCtx.getInstance(this).getDao()
        auth = AuthManager.getInstance()
        user = auth.getCurrentUser()!!
        settings = dao.getUserSettings(user.id)!!

        changes = SettingsChanges(user.username, settings.keepSignedIn)

        if (savedInstanceState == null) {
            supportFragmentManager
                .beginTransaction()
                .replace(R.id.settings, SettingsFragment(changes))
                .commit()
        }
        supportActionBar?.setDisplayHomeAsUpEnabled(true)

        PreferenceManager.getDefaultSharedPreferences(this)
            .edit()
            .putString("displayName", user.displayName)
            .putBoolean("persistSignIn", settings.keepSignedIn)
            .apply()
    }

    override fun onOptionsItemSelected(item: MenuItem): Boolean {
        val intent = Intent(applicationContext, MainActivity::class.java)
        finish()
        startActivity(intent)
        return true
    }

    override fun onPause() {
        super.onPause()

        settings.keepSignedIn = changes.persistSignIn
        val dspChange = UserDisplayUpdate(id = user.id, displayName = changes.displayName)

        dao.updateUserSettings(settings)
        dao.updateUserDisplay(dspChange)
    }

    class SettingsFragment(val changes:SettingsChanges) : PreferenceFragmentCompat() {
        override fun onCreatePreferences(savedInstanceState: Bundle?, rootKey: String?) {
            setPreferencesFromResource(R.xml.root_preferences, rootKey)

            findPreference<SwitchPreferenceCompat>("persistSignIn")?.setOnPreferenceChangeListener{ _, value ->
                changes.persistSignIn = value as Boolean
                true
            }

            findPreference<EditTextPreference>("displayName")?.setOnPreferenceChangeListener{ _, value ->
                val str = value.toString().trim()

                if(str != ""){
                    changes.displayName = str
                }

                true
            }
        }
    }

    class SettingsChanges(var displayName: String, var persistSignIn: Boolean){

    }
}