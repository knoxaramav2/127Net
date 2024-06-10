package com.knx.otscommon.secure

import org.mindrot.jbcrypt.BCrypt

class Encryption {

    class Irreversible {
        companion object{
            fun hashPassword(password:String, salt:String?=null): String{
                return BCrypt.hashpw(password, salt ?: getSalt())
            }

            @JvmStatic
            fun getSalt():String{ return BCrypt.gensalt() }

            fun checkPassword(passwordAttempt:String, hash:String): Boolean{
                return BCrypt.checkpw(passwordAttempt, hash)
            }
        }
    }

    class Reversible{

        fun saveEncrypted(fileName:String, password:String){

        }

        fun loadEncrypted(fileName:String, password: String) : String {

            return ""
        }

    }
}