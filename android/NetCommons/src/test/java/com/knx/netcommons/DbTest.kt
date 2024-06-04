package com.knx.netcommons

import android.content.Context
import androidx.room.Ignore
import androidx.room.Room
import androidx.test.core.app.ApplicationProvider
import androidx.test.filters.LargeTest
import com.knx.netcommons.Data.ConnectedDevices
import com.knx.netcommons.Data.DbCtx
import com.knx.netcommons.Data.Device
import com.knx.netcommons.Data.DeviceOwner
import com.knx.netcommons.Data.OTSDao
import com.knx.netcommons.Data.RoleAuthority
import com.knx.netcommons.Data.UserAccount
import com.knx.netcommons.Data.UserAccountAuthUpdate
import com.knx.netcommons.Data.UserAccountOperatingAuthUpdate
import com.knx.netcommons.Data.UserAccountPasswordUpdate
import org.junit.After
import org.junit.Before
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner
import secure.Encryption

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

    private fun addUser(): Int{
        val password = "MobilePassword"
        val saltHash = Encryption.Irreversible.hashPassword(password)
        val user = UserAccount(maxAuthority = 1, operatingAuthority = 2, saltedHash = saltHash)
        val userId = dao.addUserAccount(user).toInt()
        return userId
    }

    private fun addDevice(): Int{
        val device = Device(address = "7a6d:c20f:9446:87fa:ab04:f195:62e1:2900",
            hwId = "6ab396af-9ecb-4886-8def-fa4e0c082c15", os = "Test")
        val deviceId = dao.addDevice(device).toInt()
        return deviceId
    }

    @Test
    fun updatePassword(){
        val userId = addUser()
        val oldHash = dao.getUser(userId)?.saltedHash!!
        val newPassword = "NewPassword"
        val newHash = Encryption.Irreversible.hashPassword(newPassword)
        val update = UserAccountPasswordUpdate(id = userId, saltedHash = newHash)
        dao.updatePassword(update)
        val user = dao.getUser(userId)!!
        val updatedHash = user.saltedHash
        println(oldHash)
        println(newHash)
        println(updatedHash)
        assert(Encryption.Irreversible.checkPassword(newPassword, user.saltedHash))
    }

    @Test
    fun updateUserOperatingAuth(){
        val userId = addUser()
        val user = dao.getUser(id = userId)!!
        val guestAuth = dao.getAuthority("Guest")!!
        val update = UserAccountOperatingAuthUpdate(id = userId, operatingAuthority = guestAuth.id)
        dao.updateOperatingLevel(update)
        val updatedUser = dao.getUser(id = userId)!!
        val newOpRole = dao.getAuthority(authId = updatedUser.operatingAuthority)
        assert(newOpRole?.authLevel == guestAuth.authLevel)
    }

    @Test
    fun updateUserAuth(){
        val userId = addUser()
        val user = dao.getUser(id = userId)!!
        val guestAuth = dao.getAuthority("Guest")!!
        val userAuth = dao.getAuthority("User")!!

        val max = dao.getAuthority(user.maxAuthority)
        val subMax = dao.getAuthority(authId = max?.downgrade ?: -1)!!
        val op = dao.getAuthority(user.operatingAuthority)
        val subOp = dao.getAuthority(authId = op?.downgrade ?: -1)!!

        val update = UserAccountAuthUpdate(id = userId, maxAuthority = subMax.id, operatingAuthority = subOp.id)
        dao.updateUserAuth(update)

        val updatedUser = dao.getUser(userId)!!

        assert(updatedUser.maxAuthority == userAuth.id)
        assert(updatedUser.operatingAuthority == guestAuth.id)
    }


    @Test
    fun addUserDevice(){
        val userId = addUser()
        val deviceId1 = addDevice()
        val deviceId2 = addDevice()

        dao.addDeviceOwner(DeviceOwner(device = deviceId1, owner = userId))
        dao.addDeviceOwner(DeviceOwner(device = deviceId2, owner = userId))
        val res = dao.getDeviceOwnership(userId)

        assert(res.size == 2)
    }

    @Test
    fun connectDevices(){
        val device1 = addDevice()
        val device2 = addDevice()
        val connection = ConnectedDevices(device1 = device1, device2 = device2)
        dao.connectDevices(connection)

        val numCons1 = dao.getDeviceConnections(device1).size
        val numCons2 = dao.getDeviceConnections(device2).size
        assert(numCons1 > 0 && numCons1 == numCons2)
    }

    @Ignore
    @Test
    fun addProgram(){
        //TODO("addProgram")
        assert(true)
    }

    @Ignore
    @Test
    fun connectTransientDevices(){
        //TODO("connectTransientDevices")
        assert(true)
    }

    @Ignore
    @Test
    fun transferProgram(){
        //TODO("transferProgram")
        assert(true)
    }
}