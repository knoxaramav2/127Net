using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace HomeCore.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class UserAccount : IdentityUser
    {
        [ForeignKey("RoleAuthorityId")]
        public int RoleAuthorityId { get; set; }
        public required RoleAuthority OperatingAuthority { get; set; } = null!;
    }

}
