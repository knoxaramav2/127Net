package com.knx.netcommons.Data

import android.content.Context
import android.util.Log
import androidx.room.Database
import androidx.room.Room
import androidx.room.RoomDatabase
import androidx.room.TypeConverter
import androidx.room.TypeConverters
import java.util.Date

public class TimestampConverter {
    @TypeConverter
    fun fromTimeStamp(value: Long?): Date?{
        return value?.let { Date(it) }
    }

    @TypeConverter
    fun dateToTimestamp(date: Date?): Long?{
        return date?.time
    }
}

@Database(entities = [
    RoleAuthority::class, UserAccount::class,
    Device::class, DeviceOwner::class, ConnectedDevices::class,
    NetComponent::class, NetListener::class,
    NetMetaData::class, UserSettings::class, SignInLog::class,
                     //TrustContract::class,
                      //TransientCertificate::class
                     ], version=1)
@TypeConverters(TimestampConverter::class)
abstract class DbCtx : RoomDatabase(){
    abstract fun getDao(): OTSDao
    companion object {
        @Volatile
        private var instance: DbCtx? = null
        private const val DB_NAME = "OTSMobileDb"
        fun getInstance(context: Context, rebuildDb: Boolean = false) : DbCtx {
            instance ?: synchronized(this) {
                if (rebuildDb) {
                    context.deleteDatabase(DB_NAME)
                }
                instance = instance ?: Room.databaseBuilder(
                    context,
                    DbCtx::class.java, DB_NAME
                )
                    .fallbackToDestructiveMigration()
                    .allowMainThreadQueries()
                    .build()
                val dao = instance?.getDao()
                if (dao!=null){
                    initDefaults(dao)
                } else {
                    val err = "DAO Failed to build. REBUILD=$rebuildDb"
                    Log.d("LOG_INFO", err)
                    throw Exception(err)
                }
            }

            return instance!!
        }

        private fun initDefaults(dao:OTSDao){
            try{
                dao.addRoleAuthority(RoleAuthority(id = 3, roleName = "Guest", authLevel = 2))
                dao.addRoleAuthority(RoleAuthority(id = 2, roleName = "User", authLevel = 1, downgrade = 3))
                dao.addRoleAuthority(RoleAuthority(id = 1, roleName = "Admin", authLevel = 0, downgrade = 2, reauthTime = 60))
            }catch (e:Exception){
                Log.d("OTS_INFO", "Skipped initial authorities: $e")
            }

        }
    }
}

