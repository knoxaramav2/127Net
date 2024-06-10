package com.knx.otsmobile.ui.auth

import androidx.fragment.app.Fragment
import androidx.fragment.app.FragmentManager
import androidx.fragment.app.FragmentPagerAdapter
import androidx.lifecycle.Lifecycle
import androidx.viewpager2.adapter.FragmentStateAdapter
import androidx.viewpager2.widget.ViewPager2
import com.google.android.material.tabs.TabLayout
import com.google.android.material.tabs.TabLayoutMediator

/**
 * A [FragmentPagerAdapter] that returns a fragment corresponding to
 * one of the sections/tabs/pages.
 */
class AuthPagerAdapter(fm: FragmentManager, lifeCycle:Lifecycle) :
    FragmentStateAdapter(fm, lifeCycle) {

    override fun getItemCount(): Int {
        return AuthorizationPages.entries.size
    }

    override fun createFragment(position: Int): Fragment {
        return AuthFragment.newInstance(position)
    }

    fun setTab(tabLayout:TabLayout, viewPager:ViewPager2){
        TabLayoutMediator(tabLayout, viewPager){tab, pos->
            tab.text = AuthorizationPages.fromInt(pos).toString()
        }.attach()
    }
}