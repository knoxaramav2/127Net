using OTSSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;

namespace OTSStdProvider
{
    public class OTSConstantProviderTemplate(string name, Guid libGuid, OTSTypes type) : 
        OTSProviderTemplate(name, libGuid)
    {
        public override IOTSComponent CreateInstance()
        {
            return new OTSConstantProvider(Name, LibraryGuid, type);
        }
    }

    public class OTSConstantProvider : OTSProviderBase
    {
        public IOTSConfigField ValueConfig { get; set; }

        public OTSConstantProvider(string name, Guid libGuid, OTSTypes type) :
            base(name, libGuid, type)
        {
            ValueConfig = new OTSConfigField("Value", type);
            Fields = [ValueConfig];
            
        }

        public override void Update()
        {
            var value = TypeConversion.EnsureValue(ValueConfig.Get(), OTSType);
            
            Output.Value = value;
            Value = value;
        }
    }
}
