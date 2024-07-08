using OTSSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTSStdMath
{
    public class OTSMathBinaryTemplate(string name, Guid libGuid, Func<OTSData?, OTSData?, OTSData> operation) :
        OTSComponentTemplate<IOTSComponent>(name, libGuid,
            [
                new OTSInputTemplate("Input 1", OTSTypes.SIGNED),
                new OTSInputTemplate("Input 2", OTSTypes.SIGNED),
            ],
            [ new OTSOutputTemplate("Result", OTSTypes.SIGNED) ],
            [],
            false)
    {
        internal Func<OTSData?, OTSData?, OTSData> Operation = operation;

        public override OTSMathBinaryComponent CreateInstance()
        {
            return new (this, Operation);
        }

    }

    public abstract class OTSMathBase(OTSMathBinaryTemplate template, Func<OTSData?, OTSData?, OTSData> operation) : OTSComponent(template)
    {
        protected Func<OTSData?, OTSData?, OTSData> UpdateOperation { get; } = operation;
    }

    public class OTSMathBinaryComponent : OTSMathBase
    {
        public IOTSInput Input1 { get; set; }
        public IOTSInput Input2 { get; set; }
        public IOTSOutput Output { get; set; }

        public OTSMathBinaryComponent(OTSMathBinaryTemplate template,
            Func<OTSData?, OTSData?, OTSData> operation
            ) : base(template, operation)
        {
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
