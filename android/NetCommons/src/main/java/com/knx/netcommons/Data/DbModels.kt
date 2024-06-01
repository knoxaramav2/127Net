package com.knx.netcommons.Data

import androidx.room.ColumnInfo
import androidx.room.Dao
import androidx.room.Entity
import androidx.room.ForeignKey
import androidx.room.Index
import androidx.room.Insert
import androidx.room.PrimaryKey
import androidx.room.Query
import java.util.Date

const val RoleAuthorityTable : String = "roleAuthorities"
const val UserAccountTable : String = "userAccounts"
const val DeviceTable : String = "devices"
const val DeviceOwnerTable : String = "deviceOwners"
const val ConnectedDeviceTable : String = "connectedDevices"
const val NetComponentTable : String = "netComponents"
const val NetListenerTable : String = "netListeners"
const val NetMetaDataTable : String = "netMetaData"

@Entity(tableName = RoleAuthorityTable)
data class RoleAuthority(
    @PrimaryKey(autoGenerate = true) val id: Int = 0,
    @ColumnInfo(name="deletedOn") val deletedOn: Date?,
    //@ColumnInfo(name="downgrade") val downgrade: RoleAuthority?,
    @ColumnInfo(name="downgrade") val downgrade: Int?,
    @ColumnInfo(name="roleName") val roleName: String?,
    @ColumnInfo(name="authLevel") val authLevel: Int,
    @ColumnInfo(name="foreceCredential") val forceCredential: Boolean,
)

@Entity(tableName = UserAccountTable,
    foreignKeys = [
        ForeignKey(
            entity = RoleAuthority::class,
            parentColumns = ["id"],
            childColumns = ["maxAuthority"],
            onDelete = ForeignKey.RESTRICT
        ),
        ForeignKey(
            entity = RoleAuthority::class,
            parentColumns = ["id"],
            childColumns = ["operatingAuthority"],
            onDelete = ForeignKey.RESTRICT
        )
    ],
    indices = [Index(value = ["maxAuthority"]), Index(value = ["operatingAuthority"])])
data class UserAccount(
    @PrimaryKey(autoGenerate = true) val id: Int = 0,
    @ColumnInfo(name="deletedOn") val deletedOn: Date?,
    @ColumnInfo(name="maxAuthority") val maxAuthority : Int,
    @ColumnInfo(name="operatingAuthority") val operatingAuthority : Int
)

@Entity(tableName = DeviceTable)
data class Device(
    @PrimaryKey(autoGenerate = true) val id: Int = 0,
    @ColumnInfo(name="deletedOn") val deletedOn: Date?,
    @ColumnInfo(name="address") val address: String?,
    @ColumnInfo(name="friendlyName") val friendlyName: String,
    @ColumnInfo(name="displayName") val displayName: String,
    @ColumnInfo(name="hwId") val hwId: String,
    @ColumnInfo(name="manufacturer") val manufacturer: String,
    @ColumnInfo(name="os") val os: String,
)

@Entity(tableName = DeviceOwnerTable, foreignKeys = [
    ForeignKey(entity = UserAccount::class,
        parentColumns = ["id"], childColumns = ["owner"], onDelete = ForeignKey.CASCADE),
    ForeignKey(entity = Device::class,
        parentColumns = ["id"], childColumns = ["device"], onDelete = ForeignKey.CASCADE)
], indices = [Index(value = ["device"]), Index(value = ["owner"])])
data class DeviceOwner(
    @PrimaryKey(autoGenerate = true) val id: Int = 0,
    @ColumnInfo(name="deletedOn") val deletedOn: Date?,
    @ColumnInfo(name="device") val device: Int,
    @ColumnInfo(name="owner") val owner: Int,
)

@Entity(tableName = ConnectedDeviceTable, foreignKeys = [
    ForeignKey(entity = Device::class,
        parentColumns = ["id"], childColumns = ["device1"], onDelete = ForeignKey.CASCADE),
    ForeignKey(entity = Device::class,
        parentColumns = ["id"], childColumns = ["device2"], onDelete = ForeignKey.CASCADE)
])
data class ConnectedDevices(
    @PrimaryKey(autoGenerate = true) val id: Int = 0,
    @ColumnInfo(name="deletedOn") val deletedOn: Date?,
    @ColumnInfo(name="device1") val device1: Int,
    @ColumnInfo(name="device2") val device2: Int,
)

@Entity(tableName = NetComponentTable)
data class NetComponent(
    @PrimaryKey(autoGenerate = true) val id: Int = 0,
    @ColumnInfo(name="deletedOn") val deletedOn: Date?,
    @ColumnInfo(name="componentName") val componentName: String,
    @ColumnInfo(name="enabled") val enabled: String,
    @ColumnInfo(name="appData") val appData: String,
    @ColumnInfo(name="version") val version: String,
    @ColumnInfo(name="description") val description: String,
    @ColumnInfo(name="releaseTime") val releaseTime: Date,
    @ColumnInfo(name="lastUpdated") val lastUpdated: Date,
)

@Entity(tableName = NetListenerTable, foreignKeys = [
    ForeignKey(entity = Device::class,
        parentColumns = ["id"], childColumns = ["device"], onDelete = ForeignKey.RESTRICT)
])
data class NetListener(
    @PrimaryKey(autoGenerate = true) val id: Int = 0,
    @ColumnInfo(name="deletedOn") val deletedOn: Date?,
    @ColumnInfo(name="enabled") val enabled: Boolean,
    @ColumnInfo(name="device") val device: Int,
)

@Entity(tableName = NetMetaDataTable)
data class NetMetaData(
    @PrimaryKey(autoGenerate = true) val id: Int = 0,
    @ColumnInfo(name="signInDate") val signInDate: Date
)

@Dao
interface OTSDao{
    //Getters
    @Query ("SELECT * FROM $UserAccountTable")
    suspend fun getUsersAsync(): List<UserAccount>

    @Query("SELECT * FROM $UserAccountTable")
    fun getUsers(): List<UserAccount>

    @Query("SELECT * FROM $RoleAuthorityTable")
    fun getAuthorities(): List<RoleAuthority>

    @Query("SELECT COUNT() FROM $NetMetaDataTable")
    fun countSignIns(): Int

    //Inserts

    @Insert
    fun addSignIn(entry:NetMetaData)

    //Deletes

    //Updates
}
