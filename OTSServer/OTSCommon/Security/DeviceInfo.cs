using DeviceId;

namespace OTSCommon.Security
{
    public interface IDeviceInfo
    {
        string DeviceId { get; }
        string DeviceName { get; }
        string OsVersion { get; }
    };

    public class DeviceInfo : IDeviceInfo
    {
        public virtual string DeviceId { get; private set; }
        public virtual string DeviceName { get; private set; }
        public virtual string OsVersion { get; private set; }

        public DeviceInfo() 
        { 
            DeviceId = new DeviceIdBuilder()
                .AddMachineName()
                .AddOsVersion()
                .AddFileToken("./otshwid.txt")
                .ToString();

            DeviceName = new DeviceIdBuilder()
                .AddMachineName()
                .ToString();

            OsVersion = new DeviceIdBuilder()
                .AddOsVersion()
                .ToString();
        }
    }
}
