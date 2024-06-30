using OTSSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTSStdProvider
{
    #region SIGNED RANDOM
    public class SignedRandomTemplate(Guid libGuid) : OTSComponentTemplate<IOTSComponent>("SignedRandomProvider", libGuid, false)
    {
        public override SignedRandom CreateInstance() => new(Name, LibraryGuid);
        
    }

    public class SignedRandom : OTSComponent
    {
        private readonly Random Random = new();
        public IOTSData Value => new OTSData(OTSTypes.SIGNED, Random.NextInt64(MinValue, MaxValue));
        private SignedRandomConfig MinField { get; }
        private long MinValue { get { return MinField.Get()!.As<long>(); } }
        private SignedRandomConfig MaxField { get; }
        private long MaxValue { get { return MaxField.Get()!.As<long>(); } }

        public SignedRandom(string name, Guid libGuid) : base(name, libGuid)
        {
            MinField = new SignedRandomConfig("MinValue");
            MaxField = new SignedRandomConfig("MaxValue");

            Fields = [MinField, MaxField];
            Outputs = [new SignedRandomOutput(ID, this)];
        }
    }

    public class SignedRandomConfig(string name) : OTSConfigField(name, OTSTypes.SIGNED)
    {
        private long Value { get; set; } = 0;

        public override void Set(IOTSData? data)
        {
            Value = data?.As<long>() ?? 0;
        }
        public override IOTSData? Get()
        {
            return new OTSData(OTSType, Value);
        }
    }

    public class SignedRandomOutput(Guid componentId, SignedRandom host) 
        : OTSOutput("Result", componentId, OTSTypes.SIGNED)
    {
        private readonly SignedRandom _host = host;
        public override IOTSData? Get() => _host.Value;
    }

    #endregion


    #region DECIMAL RANDOM

    public class DecimalRandomTemplate(Guid libGuid) : OTSComponentTemplate<IOTSComponent>("DecimalRandomProvider", libGuid, false)
    {
        public override DecimalRandom CreateInstance() => new (Name, LibraryGuid);
    }

    public class DecimalRandom : OTSComponent
    {
        private readonly Random Random = new();
        public IOTSData Value => new OTSData(OTSTypes.DECIMAL, 
            Random.NextDouble() * (MaxValue - MinValue) + MinValue);
        private DecimalRandomConfig MinField { get; }
        private long MinValue { get { return MinField.Get()!.As<long>(); } }
        private DecimalRandomConfig MaxField { get; }
        private long MaxValue { get { return MaxField.Get()!.As<long>(); } }

        public DecimalRandom(string name, Guid libGuid) : base(name, libGuid)
        {
            MinField = new DecimalRandomConfig("MinValue");
            MaxField = new DecimalRandomConfig("MaxValue");

            Fields = [MinField, MaxField];
            Outputs = [new DecimalRandomOutput(ID, this)];
        }
    }

    public class DecimalRandomConfig(string name) : OTSConfigField(name, OTSTypes.DECIMAL)
    {
        private double Value { get; set; } = 0.0f;

        public override void Set(IOTSData? data)
        {
            Value = data?.As<double>() ?? 0;
        }

        public override IOTSData? Get() =>  new OTSData(OTSType, Value);
    }

    public class DecimalRandomOutput(Guid componentId, DecimalRandom host) 
        : OTSOutput("Result", componentId, OTSTypes.DECIMAL)
    {
        private readonly DecimalRandom _host = host;
        public override IOTSData? Get() => _host.Value;
    }

    #endregion
}
