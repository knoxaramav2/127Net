using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTSCommon.Models
{
    public class SignIn : INetRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("UserId")]
        public required UserAccount UserAccount { get; set; }
        public int UserId { get; set; }
        public DateTime SiginInDate { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
