using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace HomeCore.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class UserAccount : IdentityUser, INetRecord
    {
        public int MaxAuthorityId { get; set; }
        [ForeignKey("MaxAuthorityId")]
        public required RoleAuthority? MaxAuthority { get; set; }

        public int OperatingAuthorityId { get; set; }
        [ForeignKey("OperatingAuthorityId")]
        public required RoleAuthority? OperatingAuthority { get; set; }

        public DateTime? DeletedOn { get; set; }
    }

}
