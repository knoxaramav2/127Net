package com.knx.homemobile.ui.main

import android.os.Bundle
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Button
import android.widget.TextView
import androidx.fragment.app.Fragment
import androidx.lifecycle.ViewModelProvider
import com.knx.homemobile.databinding.FragmentTabbedAuthorizationBinding

/**
 * A placeholder fragment containing a simple view.
 */
enum class TabbedAuthorizationPages(val value: Int){
    LoginPage(0),
    RegisterPage(1);

    override fun toString(): String {
        return when(value){
            LoginPage.value -> "Login"
            RegisterPage.value -> "Register"
            else -> throw Exception("Invalid page value ($value)")
        }
    }

    companion object{
        fun fromInt(value:Int) = entries
            .first{it.value == value}
    }
}

class TabbedAuthorizationFragment : Fragment() {

    private lateinit var pageViewModel: PageViewModel
    private var _binding: FragmentTabbedAuthorizationBinding? = null

    // This property is only valid between onCreateView and
    // onDestroyView.
    private val binding get() = _binding!!

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        pageViewModel = ViewModelProvider(this)[PageViewModel::class.java].apply {
            setIndex(arguments?.getInt(ARG_SECTION_NUMBER) ?: 1)
        }
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {

        _binding = FragmentTabbedAuthorizationBinding.inflate(inflater, container, false)
        val root = binding.root

        val textView: TextView = binding.sectionLabel


        val submitBtn: Button = binding.submitBtn
        try{
            pageViewModel.pageTitle.observe(viewLifecycleOwner) { Log.d("OTS_INFO", "TITLE $it")
                textView.text = it }
            pageViewModel.submitText.observe(viewLifecycleOwner) {
                Log.d("OTS_INFO", "SUBMIT $it")
                submitBtn.text = it }
        } catch (e:Exception){
            Log.d("OTS_INFO", "Failed: $e")
        }


        return root
    }

    companion object {

        const val EXTRA_SOLE_PAGE = "sole_page"
        const val EXTRA_START_PAGE = "start_page"

        private const val ARG_SECTION_NUMBER = "section_number"

        @JvmStatic
        fun newInstance(sectionNumber: Int): TabbedAuthorizationFragment {
            return TabbedAuthorizationFragment().apply {
                arguments = Bundle().apply {
                    putInt(ARG_SECTION_NUMBER, sectionNumber)
                }
            }
        }
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }
}