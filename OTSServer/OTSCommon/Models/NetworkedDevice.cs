using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTSCommon.Models
{
    public class NetworkedDevice : INetRecord
    {
        public int Id { get; set; } 
        public required string DeviceId { get; set; }
        public required string DeviceName { get; set; }
        public required string OSVersion { get; set; }

        public DateTime? DeletedOn { get; set; }

        public ICollection<NetworkedDeviceOwner> Owners { get; } = [];
    }

    public class NetworkedDeviceOwner: INetRecord
    {
        public int Id { get; set; }
        public NetworkedDevice? Device { get; set; }
        public int DeviceId { get; set; }
        public UserAccount? Owner { get; set; }
        public int OwnerId { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
