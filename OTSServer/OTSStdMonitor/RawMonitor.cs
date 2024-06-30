using OTSSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTSStdMonitor
{
    public class RawMonitorTemplate(Guid libGuid) : OTSComponentTemplate<IOTSComponent>("RawMonitor", libGuid, true)
    {
        public override RawMonitor CreateInstance() => new (Name, LibraryGuid);
        
    }

    public class RawMonitor(string name, Guid libGuid) : OTSComponent(name, libGuid, true)
    {
    }

    public class RawInfoField(string name, OTSTypes type, bool visibility=true) : OTSConfigField(name, type, visibility)
    {
        private IOTSData? Value { get; set; } = new OTSData(type);

        public override void Set(IOTSData? data)
        {
            Value = data;
        }

        public override IOTSData? Get() =>  new OTSData(OTSType, Value);
    }
}
