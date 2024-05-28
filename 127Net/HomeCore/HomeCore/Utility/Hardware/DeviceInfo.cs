using Microsoft.AspNetCore.Identity.Data;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Management;

namespace HomeCore.Utility.Hardware
{
    public class DeviceInfo
    {
        public string DeviceName { get; private set; }
        public string? SerialNumber { get; private set; }

        private DeviceInfo() {

            DeviceName = "Unrecognized";
            SerialNumber = "XXXXXXXXXXXXXXX";

            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
#pragma warning disable 
                var hwSearch = new ManagementObjectSearcher("SELECT Product, SerialNumber FROM Win32_BaseBoard");
                var moc = hwSearch.Get();   

                DeviceName = System.Environment.MachineName;
                foreach(var mobj in moc)
                {
                    foreach(var data in mobj.Properties)
                    {
                        if(data.Name == "SerialNumber") { SerialNumber = data.Value as string; }
                    }
                }

                moc.Dispose();
            }
            else if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                throw new NotImplementedException("Device Info not implemented for Unix");
            } else
            {
                throw new NotImplementedException("Device Info not implemented for unrecognized system");
            }
        }

        public static DeviceInfo GetDeviceInfo()
        {
            return new DeviceInfo();
        }
    }
}
