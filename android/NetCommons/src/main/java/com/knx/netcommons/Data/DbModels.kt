package com.knx.netcommons.Data

import androidx.room.ColumnInfo
import androidx.room.Dao
import androidx.room.Entity
import androidx.room.ForeignKey
import androidx.room.Index
import androidx.room.Insert
import androidx.room.OnConflictStrategy
import androidx.room.PrimaryKey
import androidx.room.Query
import java.util.Date
import java.util.UUID
import kotlin.random.Random

const val RoleAuthorityTable : String = "roleAuthorities"
const val UserAccountTable : String = "userAccounts"
const val DeviceTable : String = "devices"
const val DeviceOwnerTable : String = "deviceOwners"
const val ConnectedDeviceTable : String = "connectedDevices"
const val NetComponentTable : String = "netComponents"
const val NetListenerTable : String = "netListeners"
const val NetMetaDataTable : String = "netMetaData"
const val TrustCertificateTable : String = "trustCertificate"
const val TransientCertificateTable : String = "transientCertificate"

@Entity(tableName = RoleAuthorityTable)
data class RoleAuthority(
    @PrimaryKey() val id: Int,
    @ColumnInfo(name="deletedOn") val deletedOn: Date? = null,
    @ColumnInfo(name="downgrade") val downgrade: Int? = null,
    @ColumnInfo(name="roleName") val roleName: String,
    @ColumnInfo(name="authLevel") val authLevel: Int,
    @ColumnInfo(name="reauthorizeTime") val reauthTime: Int = -1,
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
])
data class UserAccount(
    @PrimaryKey(autoGenerate = true) val id: Int = 0,
    @ColumnInfo(name="deletedOn") val deletedOn: Date? = null,
    @ColumnInfo(name="maxAuthority", index = true) val maxAuthority : Int,
    @ColumnInfo(name="operatingAuthority", index = true) val operatingAuthority : Int,
    @ColumnInfo(name="networkUserId") val networkUserId : String = UUID.randomUUID().toString(),
    @ColumnInfo(name="displayName") val displayName: String = "User",
    @ColumnInfo(name="passwordHash") val passwordHash: String? = null,
    @ColumnInfo(name="salt") val salt: Long = Random.nextLong()
)

@Entity(tableName = DeviceTable)
data class Device(
    @PrimaryKey(autoGenerate = true) val id: Int = 0,
    @ColumnInfo(name="deletedOn") val deletedOn: Date? = null,
    @ColumnInfo(name="address") val address: String? = null,
    @ColumnInfo(name="displayName") val displayName: String = "Mobile Net",
    @ColumnInfo(name="hwId") val hwId: String,
    @ColumnInfo(name="os") val os: String = "Android",
)

@Entity(tableName = DeviceOwnerTable, foreignKeys = [
    ForeignKey(entity = UserAccount::class,
        parentColumns = ["id"], childColumns = ["owner"], onDelete = ForeignKey.CASCADE),
    ForeignKey(entity = Device::class,
        parentColumns = ["id"], childColumns = ["device"], onDelete = ForeignKey.CASCADE)
], indices = [Index(name = "device_owner", value = ["device", "owner"], unique = true)])
data class DeviceOwner(
    @PrimaryKey(autoGenerate = true) val id: Int = 0,
    @ColumnInfo(name="deletedOn") val deletedOn: Date? = null,
    @ColumnInfo(name="device", index = true) val device: Int,
    @ColumnInfo(name="owner", index = true) val owner: Int,
)

@Entity(tableName = TrustCertificateTable, foreignKeys = [
    ForeignKey(entity = ConnectedDevices::class,
        parentColumns = ["device2"], childColumns = ["peerDevice"], onDelete = ForeignKey.CASCADE),
    ForeignKey(entity = RoleAuthority::class,
        parentColumns = ["id"], childColumns = ["maxAuthority"], onDelete = ForeignKey.CASCADE),
    ForeignKey(entity = Device::class,
        parentColumns = ["id"], childColumns = ["operatingAuthority"], onDelete = ForeignKey.CASCADE)
])
data class TrustContract(
    @PrimaryKey(autoGenerate = true) val id: Int = 0,
    @ColumnInfo(name = "maxAuthority", index = true) val maxAuthority: Int,
    @ColumnInfo(name = "operatingAuthority", index = true) val operatingAuthority: Int,
    @ColumnInfo(name = "peerDevice", index = true) val peerDevice: Int,
    @ColumnInfo(name = "passwordHash") val passwordHash: String? = null,
    @ColumnInfo(name = "salt") val salt: Long = Random.nextLong(),
    @ColumnInfo(name = "enabled") val enabled: Boolean = true
)

//Probably a security nightmare, revisit when less tired
@Entity(tableName = TransientCertificateTable)
data class TransientCertificate(
    @PrimaryKey(autoGenerate = true) val id: Int = 0,
    @ColumnInfo(name="deletedOn") val deletedOn: Date? = null,
    @ColumnInfo(name="issueDate") val issueDate: Date,
    @ColumnInfo(name="token") val token: String,
    @ColumnInfo(name="expirationDate") val expirationDate: Date? = null,
    @ColumnInfo(name="enabled") val enabled: Boolean = true,
)

@Entity(tableName = ConnectedDeviceTable, foreignKeys = [
    ForeignKey(entity = Device::class,
        parentColumns = ["id"], childColumns = ["device1"], onDelete = ForeignKey.CASCADE),
    ForeignKey(entity = Device::class,
        parentColumns = ["id"], childColumns = ["device2"], onDelete = ForeignKey.CASCADE),
    ForeignKey(entity = TransientCertificate::class,
        parentColumns = ["id"], childColumns = ["transientCertificate"], onDelete = ForeignKey.CASCADE),
], indices = [Index(value = ["device1", "device2"], name = "device_pair", unique = true)])
data class ConnectedDevices(
    @PrimaryKey(autoGenerate = true) val id: Int = 0,
    @ColumnInfo(name="deletedOn") val deletedOn: Date? = null,
    @ColumnInfo(name="transientCertificate", index = true) val transientCertificate: Int,
    @ColumnInfo(name="device1", index = true) val device1: Int,
    @ColumnInfo(name="device2", index = true) val device2: Int,
)

@Entity(tableName = NetComponentTable)
data class NetComponent(
    @PrimaryKey(autoGenerate = true) val id: Int = 0,
    @ColumnInfo(name="deletedOn") val deletedOn: Date? = null,
    @ColumnInfo(name="componentName") val componentName: String,
    @ColumnInfo(name="enabled") val enabled: Boolean = true,
    @ColumnInfo(name="appData") val appData: String = "",
    @ColumnInfo(name="version") val version: String = "1.0.0",
    @ColumnInfo(name="description") val description: String = "",
    @ColumnInfo(name="releaseTime") val releaseTime: Date = Date(),
    @ColumnInfo(name="lastUpdated") val lastUpdated: Date = Date(),
)

@Entity(tableName = NetListenerTable, foreignKeys = [
    ForeignKey(entity = Device::class,
        parentColumns = ["id"], childColumns = ["device"], onDelete = ForeignKey.RESTRICT)
])
data class NetListener(
    @PrimaryKey(autoGenerate = true) val id: Int = 0,
    @ColumnInfo(name="deletedOn") val deletedOn: Date? = null,
    @ColumnInfo(name="enabled") val enabled: Boolean = true,
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

    @Query("SELECT * FROM $DeviceTable")
    fun getDevices(): List<Device>

    @Query("SELECT * FROM $UserAccountTable")
    fun getUsers(): List<UserAccount>

    @Query("SELECT * FROM $RoleAuthorityTable")
    fun getAuthorities(): List<RoleAuthority>

    @Query("SELECT COUNT() FROM $NetMetaDataTable")
    fun countSignIns(): Int

    @Query("SELECT * FROM $ConnectedDeviceTable")
    fun getDeviceConnections(): List<ConnectedDevices>

    @Query("SELECT * FROM $DeviceOwnerTable WHERE owner = :userId")
    fun getDeviceOwnership(userId:Int): List<DeviceOwner>

    //Inserts
    @Insert fun addUserAccount(user:UserAccount) : Long
    @Insert fun addSignIn(entry:NetMetaData) : Long
    @Insert fun addDevice(device:Device) : Long
    @Insert fun addDeviceOwner(deviceOwner:DeviceOwner) : Long
    @Insert(onConflict = OnConflictStrategy.ABORT) fun addRoleAuthority(role:RoleAuthority) : Long

    //Deletes

    //Updates
}
