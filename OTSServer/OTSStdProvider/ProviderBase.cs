using OTSSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTSStdProvider
{
    public abstract class OTSProviderTemplate(string name, Guid libGui):
        OTSComponentTemplate<IOTSComponent>(name, libGui, false){ }

    public abstract class OTSProviderBase : OTSComponent
    {
        protected IOTSOutput Output { get; set; }
        protected IOTSData Value { get; set; }
        protected OTSTypes OTSType { get; set; }
        

        public OTSProviderBase(string name, Guid libGuid, OTSTypes type) : base(name, libGuid)
        {
            OTSType = type;
            Output = new OTSProviderOutput("Result", ID, type);
            Value = new OTSData(type);
            
            Outputs = [Output];
        }
    }

    public class OTSProviderOutput(string name, Guid componentId, OTSTypes type) : OTSOutput(name, componentId, type)
    {
    }
}
