using System.Management;

namespace NetCommons.HwDevice.Hardware
{
    public class DeviceInfo
    {
        private static DeviceInfo? Inst { get; set; }

        public string DeviceName { get; private set; } = "Unrecognized";
        public string SerialNumber { get; private set; } = "XXXXXXXXXXXXXXX";
        public string? OS { get; private set; }

        private DeviceInfo() {

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
                OS = "Unix";
                throw new NotImplementedException("Device Info not implemented for Unix");
            } else
            {
                throw new NotImplementedException("Device Info not implemented for unrecognized system");
            }
        }

        public static DeviceInfo GetDeviceInfo()
        {
            return Inst ??= new DeviceInfo();
        }
    }
}
