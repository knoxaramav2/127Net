package com.knx.homemobile.data

import android.content.Context
import secure.AuthorizeManager

class PersistState private constructor(context:Context) {
    private var currentId: Int = -1
    private val authManager = AuthorizeManager(context)

    fun currentUserId() = if (currentId == -1) null else currentId

    fun signIn(username:String, password:String): Boolean{
        val id = authManager.authenticateUserPass(username, password)
        if (id > 0){ currentId = id } else { return false }
        return true
    }

    companion object{
        private var instance: PersistState? = null

        fun getInstance(context:Context): PersistState{
            instance = instance ?: PersistState(context)
            return instance!!
        }

    }

}