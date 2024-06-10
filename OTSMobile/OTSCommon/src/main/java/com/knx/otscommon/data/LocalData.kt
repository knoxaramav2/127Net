package com.knx.otscommon.data

import android.content.Context
import java.io.Closeable
import java.io.File
import java.io.FileOutputStream
import java.util.UUID

class LocalData private constructor(ctx:Context) : Closeable{

    fun setSetupComplete(): LocalData{ completedSetup = true; return this }
    fun isSetupComplete():Boolean = completedSetup

    fun guid():String = guid

    fun save(){
        val data = arrayOf("$GUID $guid")
        val file = getLocalFile()

        if(!file.exists()){
            File(getLocalFileDir()).mkdirs()
        }

        FileOutputStream(file).use {
            writer -> data.forEach {
                val line = it+System.lineSeparator()
                writer.write(line.toByteArray())
            }
        }
    }

    fun load(): Boolean{
        val file = getLocalFile()
        if(!file.exists()) { return false }
        file.forEachLine {
            val terms = it.split("\\s+".toRegex(), limit = 2)
            when(terms[0]){
                GUID -> guid = terms[1]
            }
        }

        return true
    }

    override fun close() {
        save()
    }

    companion object{
        private var localData:LocalData? = null

        private var completedSetup : Boolean = false
        private var filesDir: String? = null
        private lateinit var guid:String

        private const val LOCAL_DIR_NAME = "OTS_data"
        private const val LOCAL_FILE_NAME = "OTSLocal.dat"
        private const val GUID = "GUID"

        private fun getLocalFileDir() = "$filesDir/$LOCAL_DIR_NAME"
        private fun getLocalFile() = File("${getLocalFileDir()}/$LOCAL_FILE_NAME")

        fun init(context:Context): LocalData{
            localData = LocalData(context)
            filesDir = context.filesDir.path

            if(!localData!!.load()){
                guid = UUID.randomUUID().toString()
                localData!!.save()
            }

            return localData!!
        }

        fun getInstance() : LocalData{
            return localData ?: throw Exception("Local data uninitialized")
        }
    }
}