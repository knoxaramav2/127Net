using OTSSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTStdLogic
{
    public abstract class OTSLogicalTemplate(string name, Guid libGuid) :
        OTSComponentTemplate<IOTSComponent>(name, libGuid, false)
    {

    }

    public abstract class OTSLogicalBase : OTSComponent
    {
        protected IOTSInput Input1 { get; set; }
        protected IOTSInput Input2 { get; set; }
        protected IOTSOutput Output { get; set; }
        protected Func<OTSData?, OTSData?, bool> UpdateOperation { get; }

        public OTSLogicalBase(string name, Guid libGuid, Func<OTSData?, OTSData?, bool> operation) : base(name, libGuid)
        {
            Input1 = new OTSInput("Input 1", ID);
            Input2 = new OTSInput("Input 2", ID);
            Output = new OTSLogicalOutput("Result", ID);

            Inputs = [Input1, Input2];
            Outputs = [Output];

            UpdateOperation = operation;
        }

        public override void Update()
        {
            var result = UpdateOperation.Invoke(
                (OTSData?)Input1.Value,
                (OTSData?)Input2.Value);
            var temp = new OTSData(OTSTypes.BOOL, result);
            Output.Value = temp;
        }
    }

    public class OTSLogicalOutput(string name, Guid libraryId) :
        OTSOutput(name, libraryId, OTSTypes.BOOL)
    {

    }
}
