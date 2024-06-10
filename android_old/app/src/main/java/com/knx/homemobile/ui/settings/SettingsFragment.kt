package com.knx.homemobile.ui.settings

import android.os.Bundle
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ArrayAdapter
import androidx.fragment.app.Fragment
import com.google.android.material.snackbar.Snackbar
import com.knx.homemobile.R
import com.knx.homemobile.databinding.FragmentSettingsBinding
import com.knx.netcommons.Data.DbCtx
import com.knx.netcommons.Data.DeviceInfo
import com.knx.netcommons.Data.LocalData
import secure.AuthorizeManager

class SettingsFragment : Fragment() {

    private var _binding: FragmentSettingsBinding? = null
    private val binding get() = _binding!!
    private lateinit var authRoles: Array<String>

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {

        _binding = FragmentSettingsBinding.inflate(inflater, container, false)
        val root: View = binding.root
        return root
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        try{
            loadPageValues()
        } catch (e:Exception) {
            Log.d("OTS_INFO", "Failed to load settings: $e")
        }

    }

    private fun loadPageValues(){
        val localData = LocalData()
        val auth = AuthorizeManager(requireContext())
        val device = DeviceInfo()
        val dao = DbCtx.getInstance(requireContext()).getDao()
        val user = auth.getAuthenticatedUser()
        if (user == null){
            activity?.let { Snackbar.make(it.findViewById(R.id.nav_host_fragment_content_main), R.string.err_user_not_authenticated, Snackbar.LENGTH_SHORT).show() }
            activity?.finish()
            return
        }

        val userName = user.username
        val address = device.getAddress()
        val displayName = user.displayName
        val maxRole = dao.getAuthority(user.maxAuthority)!!
        val opRole = dao.getAuthority(user.operatingAuthority)

        binding.userNameField.text = userName
        binding.addressField.text = address
        binding.displayNameField.setText(displayName)
        binding.maxRoleField.text = maxRole.roleName
        binding.keepSignedIn.isChecked = localData.keepSignedIn()

        var nextRoleId = maxRole.downgrade
        val tmpRoles = dao.getAuthorities()
        val localRoles = mutableListOf(maxRole.roleName)
        while(nextRoleId != null){
            tmpRoles.forEach{
                if(it.id == nextRoleId){
                    localRoles.add(it.roleName)
                    nextRoleId = it.downgrade
                    return@forEach
                }
            }
        }

        authRoles = localRoles.toTypedArray()
        val opRoleIdx = authRoles.indexOf(opRole?.roleName)
        val spinnerAdapter = ArrayAdapter(requireContext(), R.layout.fragment_settings, authRoles)
        binding.rolesListField.adapter = spinnerAdapter
        binding.rolesListField.setSelection(opRoleIdx)

        binding.submit.setOnClickListener {
            user.displayName = binding.displayNameField.text.toString()

            localData.setKeepSignIn(binding.keepSignedIn.isChecked)
        }
    }

}