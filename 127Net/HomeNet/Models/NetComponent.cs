using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace HomeNet.Models
{
    public class NetListener
    {
        [Key]
        public int Id { get; set; }
        public required Device Device { get; set; }
        public bool Enabled { get; set; }

    }

    public class NetControl
    {
        [Key]
        public int Id { get; set; }
        public required string ComponentLibId { get; set; }
        public required string FunctionId { get; set; }

    }

    public class NetComponent
    {
        [Key]
        public int Id { get; set; }
        public required string ComponentName { get; set; }
        public required bool Enabled = false;
        public required string Version { get; set; }
        public required RoleAuthority MinimumRoleAuthority { get; set; }
        public required DateTime FirstReleaseTime { get; set; }
        public required DateTime LastUpdated { get; set; }
        public List<NetControl> ComponentControls { get; set; } = [];
        public required List<NetListener> ApprovedListeners { get; set; } = [];
    }
}
