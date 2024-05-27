using Microsoft.AspNetCore.Identity.Data;
using System.Management;

namespace HomeCore.Utility.Hardware
{
    public class DeviceInfo
    {
        public string DeviceName { get; private set; }
        public string SerialNumber { get; private set; }

        private DeviceInfo() {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
#pragma warning disable 
                var hwSearch = new ManagementObjectSearcher("SELECT Product, SerialNumber FROM Win32_BaseBoard");
                var moc = hwSearch.Get();   

                foreach(var mobj in moc)
                {
                    foreach(var data in mobj.Properties)
                    {
                        Console.WriteLine($"DEV: {data.Name} > {data.Value} > {data.Origin} > {data.Qualifiers} > {data.IsLocal}");
                    }
                }

                moc.Dispose();

                DeviceName = "WIN_MACHINE_NCS_DVC_NAME";
                SerialNumber = "WIN_MACHINE_NCS_SN";
            }
            else if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                DeviceName = "LINUX_MACHINE_NCS_DVC_NAME";
                SerialNumber = "LINUX_MACHINE_NCS_SN";
            } else
            {
                DeviceName = "NOREC_MACHINE_NCS_DVC_NAME";
                SerialNumber = "NOREC_MACHINE_NCS_SN";
            }
        }

        public static DeviceInfo GetDeviceInfo()
        {
            return new DeviceInfo();
        }
    }
}
