package com.knx.homemobile.ui.main

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
class SectionsPagerAdapter(fm: FragmentManager, lifecycle: Lifecycle) :
    FragmentStateAdapter(fm, lifecycle) {

//    override fun getItem(position: Int): Fragment {
//        // getItem is called to instantiate the fragment for the given page.
//        // Return a PlaceholderFragment.
//        return TabbedAuthorizationFragment.newInstance(position + 1)
//    }
//
//    override fun getPageTitle(position: Int): CharSequence? {
//        return context.resources.getString(TAB_TITLES[position])
//    }
//
//    override fun getCount(): Int {
//        // Show 2 total pages.
//        return 2
//    }


    override fun getItemCount(): Int {
        return TabbedAuthorizationPages.entries.size
    }

    override fun createFragment(position: Int): Fragment {
        return TabbedAuthorizationFragment.newInstance(position)
    }

    fun setTab(tabLayout:TabLayout, viewPager:ViewPager2){
        TabLayoutMediator(tabLayout, viewPager){tab, position ->
            tab.text = TabbedAuthorizationPages.fromInt(position).toString()
        }.attach()
    }
}