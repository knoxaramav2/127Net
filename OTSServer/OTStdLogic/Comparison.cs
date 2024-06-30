using OTSSDK;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTStdLogic
{

    #region EQUAL

    public class LogicEqualTemplate(Guid libGuid) : OTSComponentTemplate<IOTSComponent>("LogicalEqual", libGuid, false)
    {
        public override LogicEqual CreateInstance() => new(Name, LibraryGuid);
    }

    public class LogicEqual : OTSComponent
    {
        public IOTSData Value => new OTSData(OTSTypes.BOOL, Input1Val == Input2Val);
        
        private OTSData Input1Val => Input1.Value;
        private EqualInput Input1 { get; set; }
        private OTSData Input2Val => Input2.Value;
        private EqualInput Input2 { get; set; }

        public LogicEqual(string name, Guid libGuid) : base(name, libGuid)
        {
            Input1 = new EqualInput(ID, "Input 1");
            Input2 = new EqualInput(ID, "Input 2");

            Inputs = [Input1, Input2];
            Outputs = [new EqualOutput(ID, this)];
        }
    }

    public class EqualInput(Guid componentId, string name) 
        : OTSInput(name, componentId)
    {
    }

    public class EqualOutput(Guid componentId, LogicEqual host) 
        : OTSOutput("Result", componentId, OTSTypes.BOOL)
    {
        private readonly LogicEqual _host = host;
        public override IOTSData? Get() => _host.Value;
    }

    #endregion


    #region NOT EQUAL

    public class LogicNotEqualTemplate(Guid libGuid) : OTSComponentTemplate<IOTSComponent>("LogicalNotEqual", libGuid, false)
    {
        public override LogicNotEqual CreateInstance() => new (Name, LibraryGuid);
    }

    public class LogicNotEqual : OTSComponent
    {
        public IOTSData Value => new OTSData(OTSTypes.BOOL, Input1Val != Input2Val);
        
        private OTSData Input1Val => Input1.Value;
        private NotEqualInput Input1 { get; set; }
        private OTSData Input2Val => Input2.Value;
        private NotEqualInput Input2 { get; set; }

        public LogicNotEqual(string name, Guid libGuid) : base(name, libGuid)
        {
            Input1 = new NotEqualInput(ID, "Input 1");
            Input2 = new NotEqualInput(ID, "Input 2");

            Inputs = [Input1, Input2];
            Outputs = [new NotEqualOutput(ID, this)];
        }
    }

    public class NotEqualInput(Guid componentId, string name) 
        : OTSInput(name, componentId)
    {
    }

    public class NotEqualOutput(Guid componentId, LogicNotEqual host) 
        : OTSOutput("Result", componentId, OTSTypes.BOOL)
    {
        private readonly LogicNotEqual _host = host;
        public override IOTSData? Get() => _host.Value;
    }

    #endregion


    #region LESS

    public class LogicLessTemplate(Guid libGuid) : OTSComponentTemplate<IOTSComponent>("LogicalLess", libGuid, false)
    {
        public override LogicLess CreateInstance() => new(Name, LibraryGuid);
    }

    public class LogicLess : OTSComponent
    {
        public IOTSData Value => new OTSData(OTSTypes.BOOL, Input1Val < Input2Val);
        
        private OTSData Input1Val => Input1.Value;
        private LessInput Input1 { get; set; }
        private OTSData Input2Val => Input2.Value;
        private LessInput Input2 { get; set; }

        public LogicLess(string name, Guid libGuid) : base(name, libGuid)
        {
            Input1 = new LessInput(ID, "Input 1");
            Input2 = new LessInput(ID, "Input 2");

            Inputs = [Input1, Input2];
            Outputs = [new LessOutput(ID, this)];
        }
    }

    public class LessInput(Guid componentId, string name) 
        : OTSInput(name, componentId)
    {
    }

    public class LessOutput(Guid componentId, LogicLess host) 
        : OTSOutput("Result", componentId, OTSTypes.BOOL)
    {
        private readonly LogicLess _host = host;
        public override IOTSData? Get() => _host.Value;
    }
    #endregion


    #region GREATER

    public class LogicGreaterTemplate(Guid libGuid) : OTSComponentTemplate<IOTSComponent>("LogicalGreater", libGuid, false)
    {
        public override LogicGreater CreateInstance() => new (Name, LibraryGuid);
    }

    public class LogicGreater : OTSComponent
    {
        public IOTSData Value => new OTSData(OTSTypes.BOOL, Input1Val > Input2Val);
        
        private OTSData Input1Val => Input1.Value;
        private GreaterInput Input1 { get; set; }
        private OTSData Input2Val => Input2.Value;
        private GreaterInput Input2 { get; set; }

        public LogicGreater(string name, Guid libGuid) : base(name, libGuid)
        {
            Input1 = new GreaterInput(ID, "Input 1");
            Input2 = new GreaterInput(ID, "Input 2");

            Inputs = [Input1, Input2];
            Outputs = [new GreaterOutput(ID, this)];
        }
    }

    public class GreaterInput(Guid componentId, string name) 
        : OTSInput(name, componentId)
    {
    }

    public class GreaterOutput(Guid componentId, LogicGreater host) 
        : OTSOutput("Result", componentId, OTSTypes.BOOL)
    {
        private readonly LogicGreater _host = host;
        public override IOTSData? Get() => _host.Value;
    }

    #endregion


    #region LESS EQUAL

    public class LogicLessEqualTemplate(Guid libGuid) : OTSComponentTemplate<IOTSComponent>("LogicalLessEqual", libGuid, false)
    {
        public override LogicLessEqual CreateInstance() => new (Name, LibraryGuid);
    }

    public class LogicLessEqual : OTSComponent
    {
        public IOTSData Value => new OTSData(OTSTypes.BOOL, Input1Val <= Input2Val);
        
        private OTSData Input1Val => Input1.Value;
        private LessEqualInput Input1 { get; set; }
        private OTSData Input2Val => Input2.Value;
        private LessEqualInput Input2 { get; set; }

        public LogicLessEqual(string name, Guid libGuid) : base(name, libGuid)
        {
            Input1 = new LessEqualInput(ID, "Input 1");
            Input2 = new LessEqualInput(ID, "Input 2");

            Inputs = [Input1, Input2];
            Outputs = [new LessEqualOutput(ID, this)];
        }
    }

    public class LessEqualInput(Guid componentId, string name) : OTSInput(name, componentId)
    {
    }

    public class LessEqualOutput(Guid componentId, LogicLessEqual host) 
        : OTSOutput("Result", componentId, OTSTypes.BOOL)
    {
        private readonly LogicLessEqual _host = host;
        public override IOTSData? Get() => _host.Value;
    }

    #endregion


    #region GREATER EQUAL

    public class LogicGreaterEqualTemplate(Guid libGuid) : OTSComponentTemplate<IOTSComponent>("LogicalGreaterEqual", libGuid, false)
    {
        public override LogicGreaterEqual CreateInstance() => new (Name, LibraryGuid);
    }

    public class LogicGreaterEqual : OTSComponent
    {
        public IOTSData Value => new OTSData(OTSTypes.BOOL, Input1Val >= Input2Val);
        
        private OTSData Input1Val => Input1.Value;
        private GreaterEqualInput Input1 { get; set; }
        private OTSData Input2Val => Input2.Value;
        private GreaterEqualInput Input2 { get; set; }

        public LogicGreaterEqual(string name, Guid libGuid) : base(name, libGuid)
        {
            Input1 = new GreaterEqualInput(ID, "Input 1");
            Input2 = new GreaterEqualInput(ID, "Input 2");

            Inputs = [Input1, Input2];
            Outputs = [new GreaterEqualOutput(ID, this)];
        }
    }

    public class GreaterEqualInput(Guid componentId, string name) 
        : OTSInput(name, componentId)
    {
    }

    public class GreaterEqualOutput(Guid componentId, LogicGreaterEqual host) 
        : OTSOutput("Result", componentId, OTSTypes.BOOL)
    {
        private readonly LogicGreaterEqual _host = host;
        public override IOTSData? Get() => _host.Value;
    }

    #endregion
}
