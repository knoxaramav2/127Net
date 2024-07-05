using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTSCommon.Models
{
    internal interface INetRecord
    {
        DateTime? DeletedOn { get; set; }
    }
}
