package com.knx.otsmobile.ui.network

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.Fragment
import androidx.lifecycle.ViewModelProvider
import com.knx.otsmobile.R
import com.knx.otsmobile.databinding.FragmentDevicesBinding
import com.knx.otsmobile.databinding.FragmentNetworkBinding

/**
 * A simple [Fragment] subclass.
 * Use the [NetworkFragment.newInstance] factory method to
 * create an instance of this fragment.
 */
class NetworkFragment : Fragment() {

    private var _binding: FragmentNetworkBinding? = null
    private val binding get() = _binding!!

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
//        arguments?.let {
//            param1 = it.getString(ARG_PARAM1)
//            param2 = it.getString(ARG_PARAM2)
//        }
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {

        val networkModel = ViewModelProvider(this)[NetworkViewModel::class.java]
        _binding = FragmentNetworkBinding.inflate(inflater, container, false)
        val root: View = binding.root


        val textView = binding.textNetwork
        networkModel.text.observe(viewLifecycleOwner){
            textView.text = it
        }

        return root
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }

    companion object {
        /**
         * Use this factory method to create a new instance of
         * this fragment using the provided parameters.
         *
         * @param param1 Parameter 1.
         * @param param2 Parameter 2.
         * @return A new instance of fragment NetworkFragment.
         */
        // TODO: Rename and change types and number of parameters
        @JvmStatic
        fun newInstance(param1: String, param2: String) =
            NetworkFragment().apply {
                arguments = Bundle().apply {
//                    putString(ARG_PARAM1, param1)
//                    putString(ARG_PARAM2, param2)
                }
            }
    }
}