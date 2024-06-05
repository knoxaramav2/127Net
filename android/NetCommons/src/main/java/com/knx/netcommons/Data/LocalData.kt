package com.knx.netcommons.Data

import java.io.Closeable
import java.io.File
import java.io.FileOutputStream
import java.util.UUID

class LocalData() : Closeable {

    public fun setSetupComplete(){ completedSetup = true }
    public fun isSetupComplete(): Boolean { return completedSetup }
    public fun getGuid(): String { return GUID }
    public fun getDisplayName(): String { return displayName }
    public fun getOS(): String { return "Android" }

    private fun writeData(){

        val data = arrayOf(
            "$GUID $guid", "$DISPLAY_NAME $displayName", "$COMPLETED_SETUP $completedSetup",
        )

        if(!File("$LOCAL_DIR_NAME/$LOCAL_FILE_NAME").exists()){
            File(LOCAL_DIR_NAME).mkdirs()
        }
        val file: File = File(LOCAL_DIR_NAME, LOCAL_FILE_NAME)
        FileOutputStream(file).use{
            writer ->
            data.forEach {
                val line = it+System.lineSeparator()
                writer.write(line.toByteArray()) }
        }
    }

    init{
        if(!isLoaded){
            val file = File("$LOCAL_DIR_NAME/$LOCAL_FILE_NAME")
            if (file.exists()){ loadData() }
            else { initDeviceData() }
            isLoaded = true
        }
    }

    private fun initDeviceData(){
        guid = UUID.randomUUID().toString()
    }

    private fun loadData(){
        File("$LOCAL_DIR_NAME/$LOCAL_FILE_NAME").forEachLine {
            val terms = it.split("\\s+".toRegex(), limit = 2)
            if (terms.size == 2) {
                when(terms[0]){
                    GUID -> guid = terms[1]
                    COMPLETED_SETUP -> completedSetup = terms[1] == "true"
                    DISPLAY_NAME -> displayName = terms[1]
                }
            }
        }
    }

    fun resetHard() : LocalData {
        isLoaded = false
        val file = File("$LOCAL_DIR_NAME/$LOCAL_FILE_NAME")
        if(file.exists()){ file.delete() }
        return LocalData()
    }

    override fun close() { writeData() }

    companion object{
        private var isLoaded : Boolean = false
        private var guid : String = ""
        private var completedSetup : Boolean = false
        private var displayName : String = ""

        private const val LOCAL_DIR_NAME = "OTS_data"
        private const val LOCAL_FILE_NAME = "OTSLocal.dat"

        private const val GUID = "GUID"
        private const val DISPLAY_NAME = "DisplayName"
        private const val COMPLETED_SETUP = "CompletedSetup"
    }

}