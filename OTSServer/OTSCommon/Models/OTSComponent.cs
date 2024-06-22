using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTSCommon.Models
{
    public class OTSComponent : INetRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public required string ComponentLibrary { get; set; }
        public required string ComponentProc { get; set; }

        public ICollection<OTSAppDependencies> AppDependencies { get; set; } = [];

        public DateTime? DeletedOn { get; set; }
    }

    public class OTSApplication : INetRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public ICollection<OTSAppDependencies> AppDependencies { get; set; } = [];

        public DateTime? DeletedOn { get; set; }
    }

    public class OTSAppDependencies: INetRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        //Component Info
        public int OTSComponentId { get; set; }
        public OTSComponent Component { get; set; } = null!;

        //Application Info
        public int OTSApplicationId { get; set; }
        public OTSApplication Application { get; set; } = null!;


        //Configuration Info
        public required string Configuration { get; set; }
        public required bool Enabled { get; set; }

        public DateTime? DeletedOn { get; set; }
    }

}
