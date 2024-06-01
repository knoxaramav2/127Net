using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCommons.Models
{
    public class RoleAuthority : INetRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(128)]
        public required string RoleName { get; set; }
        public int AuthLevel { get; set; }
        public bool ForceCredential { get; set; }
        public int ReauthTime { get; set; }
        public int? DowngradeId { get; set; }
        [ForeignKey(nameof(DowngradeId))]
        public RoleAuthority? Downgrade { get; set; } = null;
        public DateTime? DeletedOn { get; set; }

        //Referenced by
        public ICollection<UserAccount> OperatingAuthorities { get; } = [];
        public ICollection<UserAccount> MaximumAuthorities { get; } = [];
    }
}
