using OTSSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTStdLogic
{
    public class OTSLogicalBinaryTemplate(string name, Guid libGuid, string description,
        Func<OTSData?, OTSData?, OTSData> operation) :
        OTSComponentTemplate<IOTSComponent>(name, libGuid, description,
            [
                new OTSInputTemplate("Input 1", OTSTypes.SIGNED),
                new OTSInputTemplate("Input 2", OTSTypes.SIGNED),
            ],
            [ new OTSOutputTemplate("Result", OTSTypes.SIGNED) ],
            [],
            false)
    {
        internal Func<OTSData?, OTSData?, OTSData> Operation = operation;

        public override OTSLogicalBinaryBase CreateInstance()
        {
            return new (this, Operation);
        }

    }

    public class OTSLogicalBinaryBase : OTSComponent
    {
        public IOTSInput Input1 { get; set; }
        public IOTSInput Input2 { get; set; }
        public IOTSOutput Output { get; set; }
        protected Func<OTSData?, OTSData?, OTSData> UpdateOperation { get; }

        public OTSLogicalBinaryBase(OTSLogicalBinaryTemplate template,
            Func<OTSData?, OTSData?, OTSData> operation
            ) : base(template)
        {
            UpdateOperation = operation;
            Input1 = Inputs.First();
            Input2 = Inputs.Last();
            Output = Outputs.First();
        }

        public override void Update()
        {
            var result = UpdateOperation.Invoke(
                (OTSData?)Input1.Value,
                (OTSData?)Input2.Value);
            Output.Value = result;
        }
    }
}
