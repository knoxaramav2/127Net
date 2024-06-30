using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTSCommon.Models
{
    internal interface INetRecord
    {
        public int Id { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
