using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTSCommon.Models
{
    public class UserAccount : INetRecord
    {
        //User Information
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public required string Username { get; set; }
        public required string NormalizedUsername { get; set; }
        public required string PassHash { get; set; }
        public required string PassSalt { get; set; }

        //Max allowed permission level
        public int MaxAuthorityId { get; set; }
        public virtual required RoleAuthority MaxAuthority { get; set; }

        //Active permission level
        public int OperatingAuthorityId { get; set; }
        public virtual required RoleAuthority OperatingAuthority { get; set; }

        //Settings navigation
        public required UserSettings UserSettings { get; set; } = null!;
        public ICollection<DeviceOwner> DeviceOwners { get; set; } = [];

        public DateTime? DeletedOn { get; set; }
    }

    public class UserSettings: INetRecord
    {
        [Key]
        [ForeignKey("UserAccount")]
        public required int Id { get; set; }

        //Settings data
        public bool AutoSignIn { get; set; }

        public int EditorSettingsId { get; set; }
        public OTSSchematicEditorSettings? EditorSettings { get; set; }

        public DateTime? DeletedOn { get; set; }

        public static UserSettings GetDefaultSettings()
        {
            return new UserSettings
            {
                Id= 0,
                AutoSignIn = true,
            };
        }
    }
}
