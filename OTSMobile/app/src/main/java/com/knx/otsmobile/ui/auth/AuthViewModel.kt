package com.knx.otsmobile.ui.auth

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.map

class AuthViewModel : ViewModel() {

    private val _index = MutableLiveData<Int>()
    val pageTitle: LiveData<String> = _index.map {
        if(it == AuthorizationPages.LoginPage.value) "Sign In" else "Register Account"
    }

    val submitText: LiveData<String> = _index.map {
        AuthorizationPages.fromInt(it).toString()
    }

    val showRepeatPassword: LiveData<Boolean> = _index.map {
        it == AuthorizationPages.RegisterPage.value
    }

    val pageType: LiveData<AuthorizationPages> = _index.map {
        AuthorizationPages.fromInt(it)
    }

    fun setIndex(index: Int) {
        _index.value = index
    }
}