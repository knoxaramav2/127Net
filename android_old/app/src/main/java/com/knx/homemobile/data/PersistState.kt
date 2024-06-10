package com.knx.homemobile.data

import android.content.Context

class PersistState private constructor(context:Context) {

    companion object{
        private var instance: PersistState? = null

        fun getInstance(context:Context): PersistState{
            instance = instance ?: PersistState(context)
            return instance!!
        }

    }

}