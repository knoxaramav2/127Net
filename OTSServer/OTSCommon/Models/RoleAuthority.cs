using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTSCommon.Models
{
    public class RoleAuthority : INetRecord
    {
        public int Id { get; set; }

        public required string Name { get; set; }
        public required int AuthLevel { get; set; }
        public required bool IsDefault { get; set; }

        public ICollection<UserRole> UserRoles { get; set; } = [];

        public DateTime? DeletedOn { get; set; }
    }

    public class UserRole : INetRecord
    {
        public int Id { get; set; }

        public RoleAuthority? RoleAuthority { get; set; }
        public required int RoleAuthorityId { get; set; }

        public UserAccount? UserAccount { get; set; }
        public required int UserAccountId { get; set; }

        public required bool ActiveRole { get; set; }

        public ICollection<UserAccount> EnforcedAccounts { get; set; } = [];

        public DateTime? DeletedOn { get; set; }
    }
}
