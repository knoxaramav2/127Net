package secure

import android.content.Context
import com.knx.netcommons.Data.DbCtx
import com.knx.netcommons.Data.OTSDao
import com.knx.netcommons.Data.RoleAuthority
import com.knx.netcommons.Data.SignInLog
import com.knx.netcommons.Data.UserAccount
import com.knx.netcommons.Data.UserAccountAuthUpdate
import com.knx.netcommons.Data.UserSettings
import java.util.Date

class AuthorizeManager(ctx:Context) {
    private var dao: OTSDao = DbCtx.getInstance(ctx).getDao()

    fun tryAutoAuthenticate() : Boolean {
        val signIn = dao.getLastSignIn() ?: return false
        val settings:UserSettings? = dao.getUserSettings(signIn.userId)
        if (settings != null) {
            if (settings.keepSignedIn){
                currentUser = dao.getUser(signIn.userId)
                dao.addSignIn(SignInLog(userId = currentUser!!.id, signInDate = Date()))
                return true
            }
        }

        return false
    }

    fun verifyUserPass(username:String, password:String) : Boolean{
        val user = dao.getUser(username) ?: return false
        return Encryption.Irreversible.checkPassword(password, user.saltedHash)
    }

    fun signInUserPass(username: String, password:String): Boolean{
        if(!verifyUserPass(username, password)) { return false }
        currentUser = dao.getUser(username)
        dao.addSignIn(SignInLog(userId = currentUser!!.id, signInDate = Date()))
        return true
    }

    fun getAuthenticatedUser(): UserAccount? = currentUser

    fun registerUserPass(username: String, password:String, maxAuthority:RoleAuthority?=null): Boolean{
        if(dao.getUser(username) != null) { return false }
        val rsMaxAuthority = maxAuthority ?: dao.getAuthority("User")!!

        val passHash = Encryption.Irreversible.hashPassword(password)
        val newUser = UserAccount(maxAuthority = rsMaxAuthority.id, operatingAuthority = rsMaxAuthority.id,
            saltedHash = passHash, username = username, normalizedUsername = username.uppercase())
        val uid = dao.addUserAccount(newUser).toInt()
        val userSettings = UserSettings(id = uid, keepSignedIn = false)
        dao.addUserSettings(userSettings)

        return true
    }

    fun userExists(username: String) = dao.getUser(username) != null

    //TODO Add fix chain for broken authorize attributes
    fun updateUserAuthority(admin:String, adminPassword:String, targetUser: String, newAuthority:RoleAuthority): Boolean{
        val user = dao.getUser(targetUser)
        val opAuth = user?.let { dao.getAuthority(it.operatingAuthority) }
        val newAuth = dao.getAuthority(newAuthority.id)
        if(dao.getUser(admin) == null || user == null || opAuth == null ||
            !verifyUserPass(admin, adminPassword) || newAuth == null)
            { return false}

        val update = UserAccountAuthUpdate(id = user.id,
            maxAuthority = newAuthority.id, operatingAuthority =
            if (newAuthority.authLevel > opAuth.authLevel) newAuthority.id else opAuth.id)

        dao.updateUserAuth(update)

        return true
    }

    fun updateUserSettings(settings:UserSettings){
        dao.updateUserSettings(settings)
    }

    companion object{
        var currentUser: UserAccount? = null
    }
}