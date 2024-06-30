using OTSSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTStdLogic
{
    #region AND

    public class LogicAndTemplate : OTSComponentTemplate<IOTSComponent>
    {
        public LogicAndTemplate(Guid libGuid) : base("LogicalAnd", libGuid, false){ }

        public override LogicAnd CreateInstance()
        {
            return new LogicAnd(Name, LibraryGuid);
        }
    }

    public class LogicAnd : OTSComponent
    {
        public IOTSData Value => new OTSData(OTSTypes.BOOL, Input1Val && Input2Val);
        
        private bool Input1Val => Input1.Value.As<bool>();
        private AndInput Input1 { get; set; }
        private bool Input2Val => Input2.Value.As<bool>();
        private AndInput Input2 { get; set; }

        public LogicAnd(string name, Guid libGuid) : base(name, libGuid)
        {
            Input1 = new AndInput(ID, "Input 1");
            Input2 = new AndInput(ID, "Input 2");

            Inputs = [Input1, Input2];
            Outputs = [new AndOutput(ID, this)];
        }
    }

    public class AndInput(Guid componentId, string name) 
        : OTSInput(name, componentId, OTSTypes.BOOL)
    {
    }

    public class AndOutput(Guid componentId, LogicAnd host) 
        : OTSOutput("Result", componentId, OTSTypes.BOOL)
    {
        private readonly LogicAnd _host = host;
        public override IOTSData? Get() => _host.Value;
    }

    #endregion


    #region OR

    public class LogicOrTemplate : OTSComponentTemplate<IOTSComponent>
    {
        public LogicOrTemplate(Guid libGuid) : base("LogicalOr", libGuid, false){ }

        public override LogicOr CreateInstance()
        {
            return new LogicOr(Name, LibraryGuid);
        }
    }

    public class LogicOr : OTSComponent
    {
        public IOTSData Value => new OTSData(OTSTypes.BOOL, Input1Val || Input2Val);
        
        private bool Input1Val => Input1.Value.As<bool>();
        private OrInput Input1 { get; set; }
        private bool Input2Val => Input2.Value.As<bool>();
        private OrInput Input2 { get; set; }

        public LogicOr(string name, Guid libGuid) : base(name, libGuid)
        {
            Input1 = new OrInput(ID, "Input 1");
            Input2 = new OrInput(ID, "Input 2");

            Inputs = [Input1, Input2];
            Outputs = [new OrOutput(ID, this)];
        }
    }

    public class OrInput(Guid componentId, string name) 
        : OTSInput(name, componentId, OTSTypes.BOOL)
    {
    }

    public class OrOutput(Guid componentId, LogicOr host) 
        : OTSOutput("Result", componentId, OTSTypes.BOOL)
    {
        private readonly LogicOr _host = host;
        public override IOTSData? Get() => _host.Value;
    }

    #endregion


    #region XOR

    public class LogicXORTemplate : OTSComponentTemplate<IOTSComponent>
    {
        public LogicXORTemplate(Guid libGuid) : base("LogicalXOr", libGuid, false){ }

        public override LogicXOR CreateInstance()
        {
            return new LogicXOR(Name, LibraryGuid);
        }
    }

    public class LogicXOR : OTSComponent
    {
        public IOTSData Value => new OTSData(OTSTypes.BOOL, Input1Val ^ Input2Val);
        
        private bool Input1Val => Input1.Value.As<bool>();
        private XORInput Input1 { get; set; }
        private bool Input2Val => Input2.Value.As<bool>();
        private XORInput Input2 { get; set; }

        public LogicXOR(string name, Guid libGuid) : base(name, libGuid)
        {
            Input1 = new XORInput(ID, "Input 1");
            Input2 = new XORInput(ID, "Input 2");

            Inputs = [Input1, Input2];
            Outputs = [new XOROutput(ID, this)];
        }
    }

    public class XORInput(Guid componentId, string name) 
        : OTSInput(name, componentId, OTSTypes.BOOL)
    {
    }

    public class XOROutput(Guid componentId, LogicXOR host) 
        : OTSOutput("Result", componentId, OTSTypes.BOOL)
    {
        private readonly LogicXOR _host = host;
        public override IOTSData? Get() => _host.Value;
    }

    #endregion
}
