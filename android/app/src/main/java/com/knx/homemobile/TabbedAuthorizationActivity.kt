package com.knx.homemobile

import android.content.Intent
import android.os.Bundle
import android.view.View
import androidx.appcompat.app.AppCompatActivity
import androidx.viewpager2.widget.ViewPager2
import com.google.android.material.tabs.TabLayout
import com.knx.homemobile.databinding.ActivityTabbedAuthorizationBinding
import com.knx.homemobile.ui.main.SectionsPagerAdapter
import com.knx.homemobile.ui.main.TabbedAuthorizationFragment
import com.knx.homemobile.ui.main.TabbedAuthorizationPages

class TabbedAuthorizationActivity : AppCompatActivity() {

    private lateinit var binding: ActivityTabbedAuthorizationBinding

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        val args = AuthArgs(intent)

        binding = ActivityTabbedAuthorizationBinding.inflate(layoutInflater)
        setContentView(binding.root)

        val adapter = SectionsPagerAdapter(supportFragmentManager, lifecycle)
        val viewPager: ViewPager2 = binding.viewPager
        viewPager.adapter = adapter

        val tabs: TabLayout = binding.tabs
        adapter.setTab(tabs, viewPager)

        viewPager.currentItem = args.startPage
        if (args.solePage >= 0){
            viewPager.isUserInputEnabled = false
            TabbedAuthorizationPages.entries.forEach{
                if (it.value != args.startPage){(tabs.getTabAt(it.value)?.view)?.visibility = View.GONE}
            }
        }
    }

    class AuthArgs(intent:Intent){
        var startPage = intent.getIntExtra(TabbedAuthorizationFragment.EXTRA_START_PAGE, TabbedAuthorizationPages.LoginPage.value)
        val solePage = intent.getIntExtra(TabbedAuthorizationFragment.EXTRA_SOLE_PAGE, -1)

        init {
            if (solePage >= 0) { startPage = solePage }
        }
    }
}