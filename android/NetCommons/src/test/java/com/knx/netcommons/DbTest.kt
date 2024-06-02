package com.knx.netcommons

import android.content.Context
import androidx.room.Room
import androidx.test.core.app.ApplicationProvider
import androidx.test.filters.LargeTest
import com.knx.netcommons.Data.DbCtx
import com.knx.netcommons.Data.Device
import com.knx.netcommons.Data.DeviceOwner
import com.knx.netcommons.Data.OTSDao
import com.knx.netcommons.Data.RoleAuthority
import com.knx.netcommons.Data.UserAccount
import org.junit.After
import org.junit.Before
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner

@RunWith(RobolectricTestRunner::class)
@LargeTest
//@RunWith(AndroidJUnit4::class)
class DbTest {
    private lateinit var dao: OTSDao
    private lateinit var db: DbCtx

    @Before
    fun createDao(){
        val ctx = ApplicationProvider.getApplicationContext<Context>()
        db = Room.inMemoryDatabaseBuilder(ctx, DbCtx::class.java)
            .allowMainThreadQueries()
            .build()
        dao = db.getDao()
        dao.addRoleAuthority(RoleAuthority(id = 3, roleName = "Guest", authLevel = 2))
        dao.addRoleAuthority(RoleAuthority(id = 2, roleName = "User", authLevel = 1, downgrade = 3))
        dao.addRoleAuthority(RoleAuthority(id = 1, roleName = "Admin", authLevel = 0, downgrade = 2, reauthTime = 60))

    }

    @After
    fun exit(){
        db.close()
    }

    fun addUser(): Int{
        val user = UserAccount(maxAuthority = 1, operatingAuthority = 2)
        val userId = dao.addUserAccount(user).toInt()
        return userId
    }

    fun addDevice(): Int{
        val device = Device(address = "7a6d:c20f:9446:87fa:ab04:f195:62e1:2900", hwId = "6ab396af-9ecb-4886-8def-fa4e0c082c15", os = "Test")
        val deviceId = dao.addDevice(device).toInt()
        return deviceId
    }

    @Test
    fun addUserDevice(){
        val userId = addUser()
        val deviceId1 = addDevice()
        val deviceId2 = addDevice()
        val devices = dao.getDevices()
        val users = dao.getUsers()
        println("Users: $users | Devices: $devices")

        dao.addDeviceOwner(DeviceOwner(device = deviceId1, owner = userId))
        dao.addDeviceOwner(DeviceOwner(device = deviceId2, owner = userId))
        val res = dao.getDeviceOwnership(userId)

        println("Device connections: $res of 2")
        assert(res.size == 2)
    }
}