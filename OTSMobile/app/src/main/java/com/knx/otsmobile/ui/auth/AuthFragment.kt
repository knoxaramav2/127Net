package com.knx.otsmobile.ui.auth

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Button
import android.widget.EditText
import android.widget.TextView
import androidx.fragment.app.Fragment
import androidx.lifecycle.ViewModelProvider
import androidx.lifecycle.repeatOnLifecycle
import com.google.android.material.snackbar.Snackbar
import com.knx.otscommon.auth.AuthManager
import com.knx.otscommon.data.DbCtx
import com.knx.otscommon.data.LocalData
import com.knx.otscommon.data.OTSDao
import com.knx.otscommon.util.OTSLog
import com.knx.otsmobile.AuthActivity
import com.knx.otsmobile.IAuthArgs
import com.knx.otsmobile.R
import com.knx.otsmobile.databinding.FragmentAuthBinding

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

/**
 * A placeholder fragment containing a simple view.
 */
class AuthFragment : Fragment() {

    private lateinit var pageViewModel: AuthViewModel
    private var _binding: FragmentAuthBinding? = null
    private lateinit var  localData: LocalData
    private val binding get() = _binding!!
    private lateinit var args: AuthActivity.AuthArgs

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        localData = LocalData.getInstance()
        pageViewModel = ViewModelProvider(this)[AuthViewModel::class.java].apply {
            setIndex(arguments?.getInt(EXTRA_PAGE_IDX) ?: 1)
        }
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {

        _binding = FragmentAuthBinding.inflate(inflater, container, false)
        val root = binding.root

        val title: TextView = binding.authTitle
        val submit: Button = binding.submit
        val passRepeat: EditText = binding.repeatPasswordInput

        args = (activity as IAuthArgs).getArgs()

        pageViewModel.pageTitle.observe(viewLifecycleOwner){title.text = it}
        pageViewModel.submitText.observe(viewLifecycleOwner){submit.text = it}
        pageViewModel.showRepeatPassword.observe(viewLifecycleOwner){passRepeat.visibility = if(it)View.VISIBLE else View.GONE}
        pageViewModel.pageType.observe(viewLifecycleOwner){submit.setOnClickListener(onSubmit(it))}

        return root
    }

    private fun onSubmit(pageType: AuthorizationPages): View.OnClickListener{

        val dao = DbCtx.getInstance(requireContext()).getDao()
        val authName = if(args.asAdmin) "Admin" else "User"
        val authRole = dao.getAuthority(authName)!!
        val authMan: AuthManager = AuthManager.getInstance()


        val ctxView = activity?.findViewById<View>(R.id.view_pager)!!

        return View.OnClickListener {
            val username: String = binding.usernameInput.text.toString()
            val password: String = binding.passwordInput.text.toString()
            val passRepeat: String = binding.repeatPasswordInput.text.toString()

            if(pageType == AuthorizationPages.RegisterPage){
                if(password != passRepeat){
                    Snackbar.make(ctxView, R.string.err_password_mismatch, Snackbar.LENGTH_LONG).show()
                    return@OnClickListener
                } else if (dao.getUser(username) != null){
                    Snackbar.make(ctxView, R.string.err_username_exists, Snackbar.LENGTH_LONG).show()
                    return@OnClickListener
                } else if (!authMan.registerUser(username, password, authRole)){
                    Snackbar.make(ctxView, R.string.err_register_failed, Snackbar.LENGTH_LONG).show()
                    return@OnClickListener
                } else {
                    localData.setSetupComplete().save()
                }
            }

            if(!authMan.authenticateUserPass(username, password)){
                Snackbar.make(ctxView, R.string.err_authentication_failed, Snackbar.LENGTH_LONG).show()
            } else {
                activity?.finish()
            }
        }
    }

    companion object {

        const val EXTRA_SOLE_PAGE = "sole_page"
        const val EXTRA_START_PAGE = "start_page"
        const val EXTRA_ADMIN_REGISTER = "as_admin"
        const val EXTRA_PAGE_IDX = "page"

        @JvmStatic
        fun newInstance(sectionNumber: Int): AuthFragment {
            return AuthFragment().apply {
                arguments = Bundle().apply {
                    putInt(EXTRA_PAGE_IDX, sectionNumber)
                }
            }
        }
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }
}