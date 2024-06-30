using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OTSCommon.Models
{
    public class AppInfo : INetRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public required string AppName { get; set; }
        public required string Description { get; set; }
        public required int AuthorId { get; set; }
        public required UserAccount Author { get; set; }
        public required string AppData { get; set; }
        public required bool Enabled { get; set; }

        public DateTime? DeletedOn { get; set; }
    }

}
