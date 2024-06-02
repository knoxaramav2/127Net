package secure

import org.mindrot.jbcrypt.BCrypt

class encryption {

    companion object{
        fun hashPassword(password:String, salt:String?=null): String{
            return BCrypt.hashpw(password, salt ?: getSalt())
        }

        fun getSalt():String{ return BCrypt.gensalt() }

        fun checkPassword(passwordAttempt:String, hash:String): Boolean{
            return BCrypt.checkpw(passwordAttempt, hash)
        }
    }
}