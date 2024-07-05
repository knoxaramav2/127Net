using Microsoft.AspNetCore.Identity;

namespace OTSCommon.Models
{
    public class UserAccount : IdentityUser<int>, INetRecord
    {
        public DateTime? DeletedOn { get; set; }

        public ICollection<UserRole> Roles { get; set; } = [];
        public ICollection<NetworkedDeviceOwner> OwnedDevices { get; set; } = [];
    }
}
