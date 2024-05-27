using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeCore.Data
{
    public class Device : INetRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string DisplayName { get; set; }
        public required string HwId { get; set; }
        public required ICollection<UserAccount> Users { get; set; } = [];
        public DateTime? DeletedOn { get; set; }
    }
}
