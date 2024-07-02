using OTSSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTSStdMath
{
    public abstract class OTSMathTemplate(string name, Guid libGuid) :
        OTSComponentTemplate<IOTSComponent>(name, libGuid, false)
    {

    }

    public abstract class OTSMathBase : OTSComponent
    {
        protected IOTSInput Input1 { get; set; }
        protected IOTSInput Input2 { get; set; }
        protected IOTSOutput Output { get; set; }
        protected Func<OTSData?, OTSData?, OTSData> UpdateOperation { get; }

        public OTSMathBase(string name, Guid libGuid, Func<OTSData?, OTSData?, OTSData> operation) : base(name, libGuid)
        {
            Input1 = new OTSInput("Input 1", ID);
            Input2 = new OTSInput("Input 2", ID);
            Output = new OTSOutput("Result", ID, OTSTypes.SIGNED);

            Inputs = [Input1, Input2];
            Outputs = [Output];

            UpdateOperation = operation;
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
