package com.knx.homemobile.ui.main

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.map

class PageViewModel : ViewModel() {

    private val _index = MutableLiveData<Int>()
    val pageTitle: LiveData<String> = _index.map {
        if(it == TabbedAuthorizationPages.LoginPage.value) "Sign In" else "Register Account"
    }

    val submitText: LiveData<String> = _index.map {
        TabbedAuthorizationPages.fromInt(it).toString()
    }

    fun setIndex(index: Int) {
        _index.value = index
    }
}