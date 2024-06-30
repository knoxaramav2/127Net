using OTSSDK;
using OTSStdMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTSStdMathx
{
    public class SubtractionComponentTemplate : OTSComponentTemplate<IOTSComponent>
    {
        public SubtractionComponentTemplate(Guid libGuid): base("Subtraction", libGuid, false){ }
        public override SubtractionComponent CreateInstance()
        {
            return new SubtractionComponent(Name, LibraryGuid);
        }
    }

    public class SubtractionComponent : OTSComponent
    {
        public OTSData Sum()
        {
            var in1 = (SubInput)Inputs.ElementAt(0)!;
            var in2 = (SubInput)Inputs.ElementAt(1)!;
            var config = Fields.First();
            try
            {
                var res = checked(TrySub(in1.Value, in2.Value));
                return config.OTSType == OTSTypes.SIGNED ?
                    new OTSData(OTSTypes.SIGNED, res.As<long>()) :
                    new OTSData(OTSTypes.UNSIGNED, res.As<ulong>());
            }
            catch (Exception)
            {
                return new OTSData(config.OTSType, 0);
            }
        }

        private static OTSData TrySub(OTSData val1, OTSData val2)
        {
            if(val1.OTSType == OTSTypes.SIGNED && val2.OTSType == OTSTypes.SIGNED)
            {
                return new OTSData(OTSTypes.SIGNED, checked(val1.As<long>() - val2.As<long>()));
            } else if(val1.OTSType == OTSTypes.SIGNED && val2.OTSType == OTSTypes.UNSIGNED)
            {
                var value = checked(-(long)((ulong)-val1.As<long>() + val2.As<ulong>()));

                return value < 0 ?
                    new OTSData(OTSTypes.SIGNED, value) : 
                    new OTSData(OTSTypes.UNSIGNED, (ulong)value);
            }
            else if(val1.OTSType == OTSTypes.UNSIGNED && val2.OTSType == OTSTypes.SIGNED)
            {
                var value = checked(((long)val1.As<ulong>() - val2.As<long>()));
                return value < 0 ?
                    new OTSData(OTSTypes.SIGNED, value) : 
                    new OTSData(OTSTypes.UNSIGNED, (ulong)value);
            } else
            {
                return new OTSData(OTSTypes.UNSIGNED, checked(val1.As<ulong>() - val2.As<ulong>()));
            }
        }

        public SubtractionComponent(string name, Guid libGuid) : base(name, libGuid)
        {
            Inputs  = [new SubInput(ID, 1), new SubInput(ID,2)];
            Outputs = [new SubOutput(ID,this)];
            Fields  = [new SubSignedConfig()];
        }    
    }

    public class SubSignedConfig() : OTSConfigField("SignToggleConfig", OTSTypes.SIGNED)
    {

        public override void Set(IOTSData? data)
        {
            if(data?.OTSType != OTSTypes.BOOL) { return; }

            OTSType = data.As<bool>() ? OTSTypes.SIGNED : OTSTypes.UNSIGNED;

        } 

        public override IOTSData? Get() 
        { 
            return new OTSData(OTSTypes.BOOL, OTSType == OTSTypes.SIGNED);
        }
    }

    public class SubInput(Guid componentId, int index) 
        : OTSInput ($"Input {index}", componentId)
    {
    }

    public class SubOutput(Guid componentId, SubtractionComponent host) 
        : OTSOutput("Result", componentId, OTSTypes.SIGNED)
    {
        private readonly SubtractionComponent _host = host;
        public override IOTSData? Get() => _host.Sum();
    }
}
