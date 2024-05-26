using System.ComponentModel.DataAnnotations;

namespace HomeNet.Models
{
    public class Device
    {
        [Key]
        public int Id { get; set; }

        public required string DisplayName { get; set; }
        public required List<UserAccount> UserAccounts { get; set; }
    }
}
