package com.knx.homemobile.ui.authorization

import androidx.fragment.app.Fragment
import androidx.fragment.app.FragmentManager
import androidx.fragment.app.FragmentPagerAdapter
import androidx.lifecycle.Lifecycle
import androidx.viewpager2.adapter.FragmentStateAdapter
import androidx.viewpager2.widget.ViewPager2
import com.google.android.material.tabs.TabLayout
import com.google.android.material.tabs.TabLayoutMediator
import com.knx.homemobile.R

private val TAB_TITLES = arrayOf(
    R.string.tab_text_1,
    R.string.tab_text_2
)

/**
 * A [FragmentPagerAdapter] that returns a fragment corresponding to
 * one of the sections/tabs/pages.
 */
class AuthorizePagerAdapter(fm: FragmentManager, lifecycle: Lifecycle) :
    FragmentStateAdapter(fm, lifecycle) {

    override fun getItemCount(): Int {
        return AuthorizationPages.entries.size
    }

    override fun createFragment(position: Int): Fragment {
        return AuthorizationFragment.newInstance(position)
    }

    fun setTab(tabLayout:TabLayout, viewPager:ViewPager2){
        TabLayoutMediator(tabLayout, viewPager){tab, position ->
            tab.text = AuthorizationPages.fromInt(position).toString()
        }.attach()
    }
}