using OTSSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTSStdMonitor
{
    public class RawMonitorTemplate(Guid libGuid) : OTSMonitorTemplate<IOTSMonitor>(StdLibUtils.RawMonitor, libGuid)
    {
        public override RawMonitor CreateInstance() => new (this);
        
    }

    public class RawMonitor(RawMonitorTemplate template) : OTSMonitor(template)
    {
    }
}
