using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCommons.Models
{
    public class Device : INetRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(128)]
        public required string DisplayName { get; set; }
        [StringLength(128)]
        public required string HwId { get; set; }
        [StringLength(128)]
        public string? Manufacturer { get; set; }
        [StringLength(128)]
        public string? OS { get; set; }
        [StringLength(128)]
        public string? FriendlyName { get; set; }
        public required ICollection<UserAccount> Users { get; set; } = [];
        public DateTime? DeletedOn { get; set; }

        //Joined Devices
        public ICollection<ConnectedDevice> Connected { get; set; } = [];
    }

    public class DeviceOwner : INetRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int DeviceId { get; set; }
        [ForeignKey(nameof(DeviceId))]
        public required Device Device { get; set; }

        public required string OwnerId { get; set; }
        [ForeignKey(nameof(OwnerId))]
        public required UserAccount Owner { get; set; }

        public DateTime? DeletedOn { get; set; }
    }

    public class ConnectedDevice : INetRecord
    {   
        public int Device1Id { get; set; }
        [ForeignKey(nameof(Device1Id))]
        public required Device Device1 { get; set; }

        public int Device2Id { get; set; }
        [ForeignKey(nameof(Device2Id))]
        public required Device Device2 { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
