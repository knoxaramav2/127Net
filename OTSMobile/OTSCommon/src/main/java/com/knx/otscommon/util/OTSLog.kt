package com.knx.otscommon.util

import android.util.Log

class OTSLog {

    companion object{
        fun logInfo(msg:String){
            Log.d("OTS_INFO", msg)
        }
    }
}