package com.knx.otscommon.auth

import android.content.Context
import com.knx.otscommon.data.DbCtx
import com.knx.otscommon.data.OTSDao
import com.knx.otscommon.data.RoleAuthority
import com.knx.otscommon.data.SignInLog
import com.knx.otscommon.data.SignInLogTable
import com.knx.otscommon.data.UserAccount
import com.knx.otscommon.data.UserSettings
import com.knx.otscommon.secure.Encryption
import java.util.Date

class AuthManager private constructor(context: Context){

    private val dbCtx : DbCtx = DbCtx.getInstance(context)
    private val dao : OTSDao = dbCtx.getDao()
    private var currUser: UserAccount? = null

    fun getCurrentUser(): UserAccount? = currUser
    fun logout():Boolean {
        val settings = dao.getUserSettings(currUser!!.id) ?: return false
        settings.endSession = true
        dao.updateUserSettings(settings)
        currUser = null
        return true
    }

    fun tryAuthenticatePrevious(): Boolean{
        if (currUser != null){ return true }
        val lastLogin = dao.getLastSignIn() ?: return false
        val settings = dao.getUserSettings(lastLogin.userId)
        if (settings != null && settings.keepSignedIn && !settings.endSession){
            currUser = dao.getUser(lastLogin.userId)
            return currUser != null
        }

        return false
    }

    fun authenticateUserPass(username:String, passSalt:String): Boolean{
        val user = dao.getUser(username) ?: return false
        if (Encryption.Irreversible.checkPassword(passSalt, user.saltedHash)){
            currUser = user
            val log = SignInLog(userId = user.id, signInDate = Date())
            dao.addSignIn(log)
            val settings = dao.getUserSettings(user.id)!!
            settings.endSession = false
            dao.updateUserSettings(settings)
            return true
        }

        return false
    }

    fun registerUser(username: String, password:String, authRole:RoleAuthority): Boolean{
        var user = dao.getUser(username)
        if (user != null){ return false }

        val hash = Encryption.Irreversible.hashPassword(password)
        user = UserAccount(maxAuthority = authRole.id, operatingAuthority = authRole.id, saltedHash = hash, username = username)
        val uId = dao.addUserAccount(user).toInt()
        val settings = UserSettings(id = uId, keepSignedIn = true)
        dao.addUserSettings(settings)

        return true
    }

    companion object{

        private var instance: AuthManager? = null

        fun init(context:Context) : AuthManager{
            if (instance == null){
                instance = AuthManager(context)
            }

            return instance!!
        }

        fun getInstance() : AuthManager {
            return instance ?: throw Exception("AuthManager not initialized")
        }

    }

}