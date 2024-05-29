using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCommons.Models
{
    public class NetListener : INetRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required Device Device { get; set; }
        public bool Enabled { get; set; }
        public DateTime? DeletedOn { get; set; }
    }

    public class NetControl : INetRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string ComponentLibId { get; set; }
        public required string FunctionId { get; set; }
        public DateTime? DeletedOn { get; set; }
    }

    public class NetComponent : INetRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string ComponentName { get; set; }
        public required bool Enabled = false;
        public required string Version { get; set; }
        public required string Description { get; set; }
        public required RoleAuthority MinimumRoleAuthority { get; set; }
        public required DateTime FirstReleaseTime { get; set; }
        public required DateTime LastUpdated { get; set; }

        public required ICollection<NetControl> ComponentControls { get; set; }
        public required ICollection<NetListener> ApprovedListeners { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
