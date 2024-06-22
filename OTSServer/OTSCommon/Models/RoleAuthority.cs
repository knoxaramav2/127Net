using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTSCommon.Models
{
    public class RoleAuthority : INetRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(64)]
        public required string RoleName { get; set; }
        public required int AuthLevel { get; set; }

        public int? DowngradeId { get; set; }
        public virtual required RoleAuthority? Downgrade { get; set; }

        //Navigations
        public virtual ICollection<UserAccount> OperatingAuthorities { get; } = [];
        public virtual ICollection<UserAccount> MaximumAuthorities { get; } = [];
        public virtual ICollection<RoleAuthority> RoleAuthorities { get; } = [];

        public DateTime? DeletedOn { get; set; }
    }
}
