package com.knx.homemobile.ui.main

import android.os.Bundle
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Button
import android.widget.EditText
import android.widget.TextView
import androidx.fragment.app.Fragment
import androidx.lifecycle.ViewModelProvider
import com.google.android.material.snackbar.Snackbar
import com.knx.homemobile.R
import com.knx.homemobile.data.PersistState
import com.knx.homemobile.databinding.FragmentAuthorizationBinding
import com.knx.netcommons.Data.DbCtx
import com.knx.netcommons.Data.LocalData
import secure.AuthorizeManager

/**
 * A placeholder fragment containing a simple view.
 */
enum class AuthorizationPages(val value: Int){
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

class InvalidAuthPageException(msg:String): Exception(msg) {}

class AuthorizationFragment : Fragment() {

    private lateinit var pageViewModel: AuthorizeViewModel
    private var _binding: FragmentAuthorizationBinding? = null

    // This property is only valid between onCreateView and
    // onDestroyView.
    private val binding get() = _binding!!
    private lateinit var localData: LocalData

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        localData = LocalData()
        pageViewModel = ViewModelProvider(this)[AuthorizeViewModel::class.java].apply {
            setIndex(arguments?.getInt(ARG_SECTION_NUMBER) ?: 1)
        }
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        _binding = FragmentAuthorizationBinding.inflate(inflater, container, false)
        val root = binding.root

        val textView: TextView = binding.authTitle
        val submitBtn: Button = binding.submitBtn
        val repeatBtn: EditText = binding.passwordConfirmInput

        try{
            pageViewModel.pageTitle.observe(viewLifecycleOwner) {
                textView.text = it }
            pageViewModel.submitText.observe(viewLifecycleOwner) {
                submitBtn.text = it }
            pageViewModel.showRepeatPassword.observe(viewLifecycleOwner){
                repeatBtn.visibility = if(it) View.VISIBLE else View.GONE
            }
            pageViewModel.pageType.observe(viewLifecycleOwner){
                submitBtn.setOnClickListener(onSubmit(it))
            }

        } catch (e:Exception){
            Log.d("OTS_INFO", "Failed: $e")
        }

        return root
    }

    private fun onSubmit(pageType:AuthorizationPages): View.OnClickListener{
        val authName = if(arguments?.getBoolean(EXTRA_ADMIN_REGISTER, false) == true) "Admin" else "User"
        val dao = DbCtx.getInstance(requireContext()).getDao()
        val authRole = dao.getAuthority(authName)
        val pState = PersistState.getInstance(requireContext())

        return View.OnClickListener {

            val username = binding.usernameInput.getText().toString()
            val password = binding.passwordInput.getText().toString()
            val confirmPassword = binding.passwordConfirmInput.getText().toString()
            val authManager = AuthorizeManager(requireContext())

            Log.d("OTS_INFO","USER: $username   PASS: $password")
            val contextView = activity?.findViewById<View>(R.id.view_pager)!!

            if(pageType == AuthorizationPages.RegisterPage) {
                if(password != confirmPassword){
                    Snackbar.make(contextView, R.string.err_password_mismatch, Snackbar.LENGTH_SHORT).show()
                    return@OnClickListener
                } else if (authManager.userExists(username)){
                    Snackbar.make(contextView, R.string.err_user_exists, Snackbar.LENGTH_SHORT).show()
                    return@OnClickListener
                }
                else if (!authManager.registerUserPass(username, password, authRole)){
                    Snackbar.make(contextView, R.string.err_failed_to_register, Snackbar.LENGTH_SHORT).show()
                    return@OnClickListener
                } else{
                    LocalData().setSetupComplete()
                }
            }

            if(!pState.signIn(username, password)){
                Snackbar.make(contextView, R.string.err_failed_to_register, Snackbar.LENGTH_SHORT).show()
            } else {
                activity?.finish()
            }

        }
    }



    companion object {

        const val EXTRA_SOLE_PAGE = "sole_page"
        const val EXTRA_START_PAGE = "start_page"
        const val EXTRA_ADMIN_REGISTER = "as_admin"

        private const val ARG_SECTION_NUMBER = "section_number"

        @JvmStatic
        fun newInstance(sectionNumber: Int): AuthorizationFragment {
            return AuthorizationFragment().apply {
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