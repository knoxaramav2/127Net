using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace HomeNet.Models
{
    public class RoleAuthority
    {
        [Key]
        public int Id { get; set; }
        public required string RoleName { get; set; }
        public int AuthLevel { get; set; }
        public bool ForceCredential { get; set; }
        public DateTime ReauthTime { get; set; }
        public RoleAuthority? Downgrade { get; set; }

        //Referenced by
        //public ICollection<UserAccount?> MaximumAuthorities { get; } = [];
        public ICollection<UserAccount?> OperatingAuthorities { get; } = [];
    }
}
