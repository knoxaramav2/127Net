using OTSSDK;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTStdLogic
{

    public static class OTSCompareLogic
    {
        private static bool GoodInputs(OTSData? in1, OTSData? in2) => in1 != null && in2 != null;


        //AND OR NOT
        //TODO &&, || operators need to be fixed
        public static OTSLogicalBinaryTemplate AndTemplate(Guid libGuid) =>
            new (StdLibUtils.LogicalAnd, libGuid, "Logical And",
                (OTSData? in1, OTSData? in2) => GoodInputs(in1, in2) ?
                    new OTSData(OTSTypes.BOOL, in1!.Value.As<bool>() && in2!.Value.As<bool>()) : 
                    new OTSData(OTSTypes.BOOL, false)
                );
        public static OTSLogicalBinaryTemplate OrTemplate(Guid libGuid) =>
            new (StdLibUtils.LogicalOr, libGuid, "Logical Or",
                (OTSData? in1, OTSData? in2) => GoodInputs(in1, in2) ?
                    new OTSData(OTSTypes.BOOL, in1!.Value.As<bool>() || in2!.Value.As<bool>()) : 
                    new OTSData(OTSTypes.BOOL, false)
                );
        public static OTSLogicalBinaryTemplate XorTemplate(Guid libGuid) =>
            new (StdLibUtils.LogicalXor, libGuid, "Logical Exclusive Or",
                (OTSData? in1, OTSData? in2) => GoodInputs(in1, in2) ?
                    new OTSData(OTSTypes.BOOL, in1!.Value.As<bool>() ^ in2!.Value.As<bool>()) : 
                    new OTSData(OTSTypes.BOOL, false)
                );

        //COMPARISON
        public static OTSLogicalBinaryTemplate EqualTemplate(Guid libGuid) =>
            new (StdLibUtils.LogicalEqual, libGuid, "Equals",
                (OTSData? in1, OTSData? in2) => GoodInputs(in1, in2) ?
                    new OTSData(OTSTypes.BOOL, in1 == in2) : 
                    new OTSData(OTSTypes.BOOL, false)
                );
        
        public static OTSLogicalBinaryTemplate NotEqualTemplate(Guid libGuid) =>
            new (StdLibUtils.LogicalNotEqual, libGuid, "Not Equals",
                (OTSData? in1, OTSData? in2) => GoodInputs(in1, in2) ?
                    new OTSData(OTSTypes.BOOL, in1 != in2) : 
                    new OTSData(OTSTypes.BOOL, false)
                );

        public static OTSLogicalBinaryTemplate GreaterTemplate(Guid libGuid) =>
            new (StdLibUtils.LogicalGreater, libGuid, "Greater than",
                (OTSData? in1, OTSData? in2) => GoodInputs(in1, in2) ?
                    new OTSData(OTSTypes.BOOL, in1 > in2) : 
                    new OTSData(OTSTypes.BOOL, false)
                );

        public static OTSLogicalBinaryTemplate GreaterEqualTemplate(Guid libGuid) =>
            new (StdLibUtils.LogicalGreaterEqual, libGuid, "Greater or Equal to",
                (OTSData? in1, OTSData? in2) => GoodInputs(in1, in2) ?
                    new OTSData(OTSTypes.BOOL, in1 >= in2) : 
                    new OTSData(OTSTypes.BOOL, false)
                );

        public static OTSLogicalBinaryTemplate LessTemplate(Guid libGuid) =>
            new (StdLibUtils.LogicalLess, libGuid, "Less than",
                (OTSData? in1, OTSData? in2) => GoodInputs(in1, in2) ?
                    new OTSData(OTSTypes.BOOL, in1 < in2) : 
                    new OTSData(OTSTypes.BOOL, false)
                );

        public static OTSLogicalBinaryTemplate LessEqualTemplate(Guid libGuid) =>
            new (StdLibUtils.LogicalLessEqual, libGuid, "Less or Equal to",
                (OTSData? in1, OTSData? in2) => GoodInputs(in1, in2) ?
                    new OTSData(OTSTypes.BOOL, in1 <= in2) : 
                    new OTSData(OTSTypes.BOOL, false)
                );
    }
}
