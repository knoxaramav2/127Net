package com.knx.netcommons.Data

import android.content.Context
import android.util.Log
import java.io.Closeable
import java.io.File
import java.io.FileOutputStream
import java.util.UUID

class LocalData(ctx:Context?=null) : Closeable {

    fun setSetupComplete(): LocalData{ completedSetup = true; return this }
    fun isSetupComplete(): Boolean = completedSetup
    fun keepSignedIn(): Boolean = keepSignedIn
    fun setKeepSignIn(checked:Boolean) { keepSignedIn = checked }
    fun getGuid(): String { return GUID }
    fun getOS(): String { return "Android" }

    fun save(){
        writeData()
    }

    init{

        if(filesDir == null){
            if (ctx == null){ throw Exception("Context must be set at least once before accessing local data") }
            filesDir = ctx.filesDir.path
        }

        if(!isLoaded){
            val file = getLocalFile()
            if (file.exists()){ loadData() }
            else {
                initDeviceData()
                writeData()
            }
            isLoaded = true
        }
    }

    private fun initDeviceData(){
        guid = UUID.randomUUID().toString()
    }

    private fun loadData(){
        val file = getLocalFile()
        file.forEachLine {
            val terms = it.split("\\s+".toRegex(), limit = 2)
            if (terms.size == 2) {
                when(terms[0]){
                    GUID -> guid = terms[1]
                    COMPLETED_SETUP -> completedSetup = terms[1] == "true"
                    KEEP_SIGN_IN -> keepSignedIn = terms[1] == "true"
                }
            }
        }
    }

    private fun writeData(){

        val data = arrayOf(
            "$GUID $guid", "$COMPLETED_SETUP $completedSetup", "$KEEP_SIGN_IN $keepSignedIn"
        )

        val file = getLocalFile()

        if(!file.exists()){
            val createPath = "TestDir"
            if(!File("TestDir").mkdirs()){
                Log.d("OTS_LOG", "Failed to created path $createPath")
            } else {
                Log.d("OTS_LOG", "Created: $createPath")
            }
        }
        FileOutputStream(file).use{
                writer ->
            data.forEach {
                val line = it+System.lineSeparator()
                writer.write(line.toByteArray()) }
        }
    }


    fun resetHard() : LocalData {
        isLoaded = false
        val file = getLocalFile()
        if(file.exists()){ file.delete() }
        return LocalData()
    }

    private fun getLocalFileDir() = "$filesDir/$LOCAL_DIR_NAME"
    private fun getLocalFile() = File("${getLocalFileDir()}/$LOCAL_FILE_NAME")

    override fun close() { writeData() }

    companion object{
        private var isLoaded : Boolean = false
        private var guid : String = ""
        private var completedSetup : Boolean = false
        private var keepSignedIn : Boolean = false

        private var filesDir: String? = null

        private const val LOCAL_DIR_NAME = "OTS_data"
        private const val LOCAL_FILE_NAME = "OTSLocal.dat"

        private const val GUID = "GUID"
        private const val COMPLETED_SETUP = "CompletedSetup"
        private const val KEEP_SIGN_IN = "PersistSignIn"
    }

}