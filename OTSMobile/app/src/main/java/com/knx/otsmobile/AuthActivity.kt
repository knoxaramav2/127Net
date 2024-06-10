package com.knx.otsmobile

import android.content.Intent
import android.os.Bundle
import android.view.View
import androidx.appcompat.app.AppCompatActivity
import androidx.viewpager2.widget.ViewPager2
import com.google.android.material.tabs.TabLayout
import com.knx.otsmobile.databinding.ActivityAuthBinding
import com.knx.otsmobile.ui.auth.AuthFragment
import com.knx.otsmobile.ui.auth.AuthPagerAdapter
import com.knx.otsmobile.ui.auth.AuthorizationPages

interface IAuthArgs{
    fun getArgs(): AuthActivity.AuthArgs
}

class AuthActivity : AppCompatActivity(), IAuthArgs {

    private lateinit var binding: ActivityAuthBinding
    private lateinit var args: AuthArgs

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        args = AuthArgs(intent)

        binding = ActivityAuthBinding.inflate(layoutInflater)
        setContentView(binding.root)

        val adapter = AuthPagerAdapter(supportFragmentManager, lifecycle)
        val viewPager:ViewPager2 = binding.viewPager
        viewPager.adapter = adapter

        val tabs: TabLayout = binding.tabs
        adapter.setTab(tabs, viewPager)
        viewPager.currentItem = args.startPage

        if(args.solePage >= 0){
            viewPager.isUserInputEnabled = false
            AuthorizationPages.entries.forEach{
                if(it.value != args.startPage){(tabs.getTabAt(it.value)?.view)?.visibility = View.GONE}
            }
        }
    }

    class AuthArgs(intent: Intent){
        var startPage = intent.getIntExtra(AuthFragment.EXTRA_START_PAGE, AuthorizationPages.LoginPage.value)
        val solePage = intent.getIntExtra(AuthFragment.EXTRA_SOLE_PAGE, -1)
        val asAdmin = intent.getBooleanExtra(AuthFragment.EXTRA_ADMIN_REGISTER, false)

        init {
            if (solePage >= 0) { startPage = solePage }
        }
    }

    override fun getArgs() = args
}