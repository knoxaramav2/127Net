using OTSSDK;
using System.Runtime.CompilerServices;

namespace OTSStdMath
{
    public class AdditionComponentTemplate : OTSComponentTemplate<IOTSComponent>
    {
        public AdditionComponentTemplate(Guid libGuid): base("Addition", libGuid, false){ }
        public override AdditionComponent CreateInstance()
        {
            return new AdditionComponent(Name, LibraryGuid);
        }
    }

    public class AdditionComponent : OTSComponent
    {
        public OTSData Sum()
        {
            var in1 = (AddInput)Inputs.ElementAt(0)!;
            var in2 = (AddInput)Inputs.ElementAt(1)!;
            var config = Fields.First();
            try
            {
                var res = TryAdd(in1.Value, in2.Value);
                return config.OTSType == OTSTypes.SIGNED ?
                    new OTSData(OTSTypes.SIGNED, res.As<long>()) :
                    new OTSData(OTSTypes.UNSIGNED, res.As<ulong>());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new OTSData(config.OTSType, 0);
            }
        }

        private static OTSData TryAdd(OTSData val1, OTSData val2)
        {
            if(val1.OTSType == OTSTypes.SIGNED && val2.OTSType == OTSTypes.SIGNED)
            {
                return new OTSData(OTSTypes.SIGNED, checked(val1.As<long>() + val2.As<long>()));
            } else if(val1.OTSType == OTSTypes.SIGNED && val2.OTSType == OTSTypes.UNSIGNED)
            {
                var value = checked(val2.As<ulong>() - (ulong)-val1.As<long>());
                return value < 0 ?
                    new OTSData(OTSTypes.SIGNED, (long)value) : 
                    new OTSData(OTSTypes.UNSIGNED, value);
            }
            else if(val1.OTSType == OTSTypes.UNSIGNED && val2.OTSType == OTSTypes.SIGNED)
            {
                var value = checked((long)val1.As<ulong>() + val2.As<long>());
                return value < 0 ?
                    new OTSData(OTSTypes.SIGNED, value) : 
                    new OTSData(OTSTypes.UNSIGNED, (ulong)value);
            } else
            {
                return new OTSData(OTSTypes.UNSIGNED, checked(val1.As<long>() + val2.As<long>()));
            }
        }

        public AdditionComponent(string name, Guid libGuid) : base (name, libGuid)
        {
            Inputs  = [new AddInput(ID, 1), new AddInput(ID,2)];
            Outputs = [new AddOutput(ID, this)];
            Fields  = [new AddSignedConfig()];
        }    
    }

    public class AddSignedConfig() : OTSConfigField("SignToggleConfig", OTSTypes.SIGNED)
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

    public class AddInput(Guid componentId, int index) 
        : OTSInput($"Input {index}", componentId)
    {
    }

    public class AddOutput(Guid componentId, AdditionComponent host) 
        : OTSOutput("Result", componentId, OTSTypes.SIGNED)
    {
        private readonly AdditionComponent _host = host;
        public override IOTSData? Get() => _host.Sum();
    }
}
