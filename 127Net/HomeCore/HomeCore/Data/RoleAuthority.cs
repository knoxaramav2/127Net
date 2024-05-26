using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeCore.Data
{
    public class RoleAuthority
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string RoleName { get; set; }
        public int AuthLevel { get; set; }
        public bool ForceCredential { get; set; }
        public DateTime ReauthTime { get; set; }
        public RoleAuthority? Downgrade { get; set; } = null;

        //Referenced by
        public ICollection<UserAccount> OperatingAuthorities { get; } = [];
    }
}
