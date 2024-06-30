using OTSSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;

namespace OTSStdProvider
{
    #region SIGNED
    public class SignedConstantTemplate(Guid libGuid) : OTSComponentTemplate<IOTSComponent>("SignedProvider", libGuid, false)
    {
        public override SignedConstant CreateInstance() => new (Name, LibraryGuid);
    }

    public class SignedConstant : OTSComponent
    {
        public IOTSData Value => ValueField.Get()!;
        private SignedConstConfig ValueField { get; }

        public SignedConstant(string name, Guid libGuid) : base(name, libGuid)
        {
            ValueField = new SignedConstConfig();

            Fields = [ValueField];
            Outputs = [new SignedConstOutput(ID, this)];
        }
    }

    public class SignedConstConfig() : OTSConfigField("Value", OTSTypes.SIGNED)
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

    public class SignedConstOutput(Guid componentId, SignedConstant host) 
        : OTSOutput("Result", componentId, OTSTypes.SIGNED)
    {
        private readonly SignedConstant _host = host;
        public override IOTSData? Get() => _host.Value;
    }

    #endregion


    #region #UNSIGNED

    public class UnsignedConstantTemplate(Guid libGuid) : OTSComponentTemplate<IOTSComponent>("UnsignedProvider", libGuid, false)
    {
        public override UnsignedConstant CreateInstance() => new (Name, LibraryGuid);
    }

    public class UnsignedConstant : OTSComponent
    {
        public IOTSData Value => ValueField.Get()!;
        private UnsignedConstConfig ValueField { get; }

        public UnsignedConstant(string name, Guid libGuid) : base(name, libGuid)
        {
            ValueField = new UnsignedConstConfig();

            Fields = [ValueField];
            Outputs = [new UnsignedConstOutput(ID, this)];
        }
    }

    public class UnsignedConstConfig() : OTSConfigField("Value", OTSTypes.UNSIGNED)
    {
        private ulong Value { get; set; } = 0;

        public override void Set(IOTSData? data)
        {
            Value = data?.As<ulong>() ?? 0;
        }
        public override IOTSData? Get()
        {
            return new OTSData(OTSType, Value);
        }
    }

    public class UnsignedConstOutput(Guid componentId, UnsignedConstant host) 
        : OTSOutput("Result", componentId, OTSTypes.UNSIGNED)
    {
        private readonly UnsignedConstant _host = host;
        public override IOTSData? Get() => _host.Value;
    }

    #endregion


    #region #DOUBLE

    public class DecimalConstantTemplate(Guid libGuid) : OTSComponentTemplate<IOTSComponent>("DecimalProvider", libGuid, false)
    {
        public override DecimalConstant CreateInstance() => new (Name, LibraryGuid);
    }

    public class DecimalConstant : OTSComponent
    {
        public IOTSData Value => ValueField.Get()!;
        private DecimalConstConfig ValueField { get; }

        public DecimalConstant(string name, Guid libGuid) : base(name, libGuid)
        {
            ValueField = new DecimalConstConfig();

            Fields = [ValueField];
            Outputs = [new DecimalConstOutput(ID, this)];
        }
    }

    public class DecimalConstConfig() : OTSConfigField("Value", OTSTypes.DECIMAL)
    {
        private double Value { get; set; } = 0;

        public override void Set(IOTSData? data)
        {
            Value = data?.As<double>() ?? 0;
        }
        public override IOTSData? Get()
        {
            return new OTSData(OTSType, Value);
        }
    }

    public class DecimalConstOutput(Guid componentId, DecimalConstant host) 
        : OTSOutput("Result", componentId, OTSTypes.DECIMAL)
    {
        private readonly DecimalConstant _host = host;
        public override IOTSData? Get() => _host.Value;
    }

    #endregion


    #region #BOOL

    public class BoolConstantTemplate(Guid libGuid) : OTSComponentTemplate<IOTSComponent>("BoolProvider", libGuid, false)
    {
        public override BoolConstant CreateInstance() => new (Name, LibraryGuid);
    }

    public class BoolConstant : OTSComponent
    {
        public IOTSData Value => ValueField.Get()!;
        private BoolConstConfig ValueField { get; }

        public BoolConstant(string name, Guid libGuid) : base(name, libGuid)
        {
            ValueField = new BoolConstConfig();

            Fields = [ValueField];
            Outputs = [new BoolConstOutput(ID, this)];
        }
    }

    public class BoolConstConfig() : OTSConfigField("Value", OTSTypes.BOOL)
    {
        private bool Value { get; set; } = false;

        public override void Set(IOTSData? data)
        {
            Value = data?.As<bool>() ?? false;
        }
        public override IOTSData? Get()
        {
            return new OTSData(OTSType, Value);
        }
    }

    public class BoolConstOutput(Guid componentId, BoolConstant host) 
        : OTSOutput("Result", componentId, OTSTypes.BOOL)
    {
        private readonly BoolConstant _host = host;
        public override IOTSData? Get() => _host.Value;
    }

    #endregion


    #region #STRING

    public class StringConstantTemplate(Guid libGuid) : OTSComponentTemplate<IOTSComponent>("StringProvider", libGuid, false)
    {
        public override StringConstant CreateInstance() => new (Name, LibraryGuid);
    }

    public class StringConstant : OTSComponent
    {
        public IOTSData Value => ValueField.Get()!;
        private StringConstConfig ValueField { get; }

        public StringConstant(string name, Guid libGuid) : base(name, libGuid)
        {
            ValueField = new StringConstConfig();

            Fields = [ValueField];
            Outputs = [new StringConstOutput(ID, this)];
        }
    }

    public class StringConstConfig() : OTSConfigField("Value", OTSTypes.STRING)
    {
        private string Value { get; set; } = string.Empty;

        public override void Set(IOTSData? data)
        {
            Value = data?.As<string>() ?? string.Empty;
        }
        public override IOTSData? Get()
        {
            return new OTSData(OTSType, Value);
        }
    }

    public class StringConstOutput(Guid componentId, StringConstant host) 
        : OTSOutput("Result", componentId, OTSTypes.STRING)
    {
        private readonly StringConstant _host = host;
        public override IOTSData? Get() => _host.Value;
    }

    #endregion

    #region #LIST

    public class ListConstantTemplate(Guid libGuid) : OTSComponentTemplate<IOTSComponent>("ListProvider", libGuid, false)
    {
        public override ListConstant CreateInstance() => new (Name, LibraryGuid);
    }

    public class ListConstant : OTSComponent
    {
        public IOTSData Value => ValueField.Get()!;
        private ListConstConfig ValueField { get; }

        public ListConstant(string name, Guid libGuid) : base(name, libGuid)
        {
            ValueField = new ListConstConfig();

            Fields = [ValueField];
            Outputs = [new ListConstOutput(ID, this)];
        }
    }

    public class ListConstConfig() : OTSConfigField("Value", OTSTypes.LIST)
    {
        private List<object> Value { get; set; } = [];

        public override void Set(IOTSData? data)
        {
            Value = data?.As<List<object>>() ?? [];
        }
        public override IOTSData? Get()
        {
            return new OTSData(OTSType, Value);
        }
    }

    public class ListConstOutput(Guid componentId, ListConstant host) 
        : OTSOutput("Result", componentId, OTSTypes.LIST)
    {
        private readonly ListConstant _host = host;
        public override IOTSData? Get() => _host.Value;
    }

    #endregion

    #region #MAP

    public class MapConstantTemplate(Guid libGuid) : OTSComponentTemplate<IOTSComponent>("MapProvider", libGuid, false)
    {
        public override MapConstant CreateInstance() => new (Name, LibraryGuid);
    }

    public class MapConstant : OTSComponent
    {
        public IOTSData Value => ValueField.Get()!;
        private MapConstConfig ValueField { get; }

        public MapConstant(string name, Guid libGuid) : base(name, libGuid)
        {
            ValueField = new MapConstConfig();

            Fields = [ValueField];
            Outputs = [new MapConstOutput(ID, this)];
        }
    }

    public class MapConstConfig() : OTSConfigField("Value", OTSTypes.MAP)
    {
        private Dictionary<string, object> Value { get; set; } = [];

        public override void Set(IOTSData? data)
        {
            Value = data?.As<Dictionary<string, object>>() ?? [];
        }
        public override IOTSData? Get()
        {
            return new OTSData(OTSType, Value);
        }
    }

    public class MapConstOutput(Guid componentId, MapConstant host) 
        : OTSOutput("Result", componentId, OTSTypes.MAP)
    {
        private readonly MapConstant _host = host;
        public override IOTSData? Get() => _host.Value;
    }

    #endregion
}
