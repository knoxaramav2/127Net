package com.knx.homemobile

import android.content.Intent
import android.os.Bundle
import android.view.View
import androidx.appcompat.app.AppCompatActivity
import androidx.viewpager2.widget.ViewPager2
import com.google.android.material.tabs.TabLayout
import com.knx.homemobile.databinding.ActivityAuthorizationBinding
import com.knx.homemobile.ui.authorization.AuthorizationFragment
import com.knx.homemobile.ui.authorization.AuthorizationPages
import com.knx.homemobile.ui.authorization.AuthorizePagerAdapter

interface IAuthArgs{
    fun getArgs(): AuthorizationActivity.AuthArgs
}

class AuthorizationActivity : AppCompatActivity(), IAuthArgs {

    private lateinit var binding: ActivityAuthorizationBinding
    private lateinit var args: AuthArgs

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        args = AuthArgs(intent)

        binding = ActivityAuthorizationBinding.inflate(layoutInflater)
        setContentView(binding.root)

        val adapter = AuthorizePagerAdapter(supportFragmentManager, lifecycle)
        val viewPager: ViewPager2 = binding.viewPager
        viewPager.adapter = adapter

        val tabs: TabLayout = binding.tabs
        adapter.setTab(tabs, viewPager)

        viewPager.currentItem = args.startPage
        if (args.solePage >= 0){
            viewPager.isUserInputEnabled = false
            AuthorizationPages.entries.forEach{
                if (it.value != args.startPage){(tabs.getTabAt(it.value)?.view)?.visibility = View.GONE}
            }
        }
    }

    class AuthArgs(intent:Intent){
        var startPage = intent.getIntExtra(AuthorizationFragment.EXTRA_START_PAGE, AuthorizationPages.LoginPage.value)
        val solePage = intent.getIntExtra(AuthorizationFragment.EXTRA_SOLE_PAGE, -1)
        val asAdmin = intent.getBooleanExtra(AuthorizationFragment.EXTRA_ADMIN_REGISTER, false)

        init {
            if (solePage >= 0) { startPage = solePage }
        }
    }

    override fun getArgs() = args
}