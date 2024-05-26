using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace HomeNet.Models
{
    public partial class UserAccount
    {
        [Key]
        public int Id { get; set; }
        //[AllowNull]
        //public RoleAuthority? MaximumAuthority { get; set; }
        [AllowNull]
        public RoleAuthority? OperatingAuthority { get; set; }
        [NotNull]
        public required string UserName { get; set; }
        [AllowNull]
        public string Email { get; set; }
        [AllowNull]
        public string PhoneNumber { get; set; }
    }
}
