using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeCore.Data
{
    public class RoleAuthority : INetRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string RoleName { get; set; }
        public int AuthLevel { get; set; }
        public bool ForceCredential { get; set; }
        public int ReauthTime { get; set; }
        public RoleAuthority? Downgrade { get; set; } = null;
        public DateTime? DeletedOn { get; set; }

        //Referenced by
        public ICollection<UserAccount> OperatingAuthorities { get; } = [];
        public ICollection<UserAccount> MaximumAuthorities { get; } = [];
    }
}
