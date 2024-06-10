package com.knx.otscommon.data

import android.content.Context
import android.net.wifi.WifiManager
import android.text.format.Formatter
import com.knx.otscommon.util.OTSLog
import java.net.NetworkInterface
import java.text.Format
import java.text.Normalizer.Form
import java.util.Collections

class DeviceInfo private constructor(){
    private var _macAddress: String = ""
    private var _ipv4: String = ""
    private var _ipv6: String = ""

    val macAddress: String get() = _macAddress
    val ipv4: String get() = _ipv4
    val ipv6: String get() = _ipv6

    init{
        _macAddress = getMacAddress()
        _ipv4 = getIpAddress(true)
        _ipv6 = getIpAddress(false)
        OTSLog.logInfo("IPV4: $ipv4  |  IPV6: $ipv6")
    }

    private fun getInterfaces(): ArrayList<NetworkInterface> = Collections.list(NetworkInterface.getNetworkInterfaces())
    private fun getInetAddresses(intf: NetworkInterface) = Collections.list(intf.inetAddresses)

    private fun getMacAddress(interfaceName:String="wlan0"): String{
        try{
            val interfaces = getInterfaces()
            interfaces.forEach{ it ->
                if(it.name.equals(interfaceName, true)){ return@forEach }
                val mac = it.hardwareAddress
                return if (mac == null) "" else {
                    val buf = StringBuilder()
                    mac.forEach { buf.append(String.format("%02X:", it)) }
                    if(buf.isNotEmpty()){ buf.deleteCharAt(buf.length-1) }
                    buf.toString()
                    }
                }
            }
        catch (e:Exception){
            OTSLog.logInfo("Failed to get MAC address for $interfaceName")
        }

        return ""
    }

    private fun getIpAddress(useIpv4:Boolean): String{
        try{
            val interfaces = getInterfaces()
            interfaces.forEach { it ->
                val addrs = getInetAddresses(it)
                addrs.forEach{ addr ->
                    if (!addr.isLoopbackAddress){
                        val hostAddr = addr.hostAddress ?: ""
                        val isIpv4 = !hostAddr.contains(':')

                        if (useIpv4) { return if(isIpv4) hostAddr else "" }
                        else if (!isIpv4) {
                            val delim = hostAddr.indexOf('%')
                            return if(delim < 0) hostAddr.uppercase() else hostAddr.substring(0, delim).uppercase()
                        }
                    }
                }
            }
        } catch (e:Exception){
            OTSLog.logInfo("Failed to get device IP address")
        }

        return ""
    }

    companion object{
        private var instance: DeviceInfo? = null

        fun getInstance(): DeviceInfo{
            if (instance == null) { instance = DeviceInfo() }
            return instance ?: throw Exception("Device not instantiated")
        }
    }

}