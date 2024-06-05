package secure

import android.content.Context
import com.knx.netcommons.Data.DbCtx
import com.knx.netcommons.Data.OTSDao
import com.knx.netcommons.Data.RoleAuthority
import com.knx.netcommons.Data.UserAccount
import com.knx.netcommons.Data.UserAccountAuthUpdate

class AuthorizeManager(ctx:Context) {
    private var dao: OTSDao = DbCtx.getInstance(ctx).getDao()

    fun authenticateUserPass(username:String, password:String) : Int{
        val user = dao.getUser(username) ?: return -1
        return if(Encryption.Irreversible.checkPassword(password, user.saltedHash))
            user.id else -1
    }

    fun registerUserPass(username: String, password:String, maxAuthority:RoleAuthority?=null): Boolean{
        if(dao.getUser(username) != null) { return false }
        val rsMaxAuthority = maxAuthority ?: dao.getAuthority("User")!!

        val passHash = Encryption.Irreversible.hashPassword(password)
        val newUser = UserAccount(maxAuthority = rsMaxAuthority.id, operatingAuthority = rsMaxAuthority.id,
            saltedHash = passHash, username = username, normalizedUsername = username.uppercase())
        dao.addUserAccount(newUser)

        return true
    }

    fun userExists(username: String) = dao.getUser(username) != null

    //TODO Add fix chain for broken authorize attributes
    fun updateUserAuthority(admin:String, adminPassword:String, targetUser: String, newAuthority:RoleAuthority): Boolean{
        val user = dao.getUser(targetUser)
        val opAuth = user?.let { dao.getAuthority(it.operatingAuthority) }
        val newAuth = dao.getAuthority(newAuthority.id)
        if(dao.getUser(admin) == null || user == null || opAuth == null ||
            authenticateUserPass(admin, adminPassword) != -1 || newAuth == null)
            { return false}

        val update = UserAccountAuthUpdate(id = user.id,
            maxAuthority = newAuthority.id, operatingAuthority =
            if (newAuthority.authLevel > opAuth.authLevel) newAuthority.id else opAuth.id)

        dao.updateUserAuth(update)

        return true
    }
}