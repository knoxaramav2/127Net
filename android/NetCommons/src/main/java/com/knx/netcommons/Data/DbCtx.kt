package com.knx.netcommons.Data

import android.content.Context
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
    NetMetaData::class,
                     ], version=3)
@TypeConverters(TimestampConverter::class)
abstract class DbCtx : RoomDatabase(){

    abstract fun getDao(): OTSDao
    companion object {
        @Volatile
        private var instance: DbCtx? = null

        fun getInstance(context: Context) =
            instance ?: synchronized(this){
                instance ?: Room.databaseBuilder(context,
                    DbCtx::class.java, "OTSMobileDb")
                    .fallbackToDestructiveMigration()
                    .allowMainThreadQueries()
                    .build()
            }
    }
}

