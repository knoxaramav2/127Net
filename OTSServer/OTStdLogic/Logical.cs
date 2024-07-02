using OTSSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTStdLogic
{
    public class OTSLogicalAndTemplate(Guid libGuid) : OTSLogicalTemplate(StdLibUtils.LogicalAnd, libGuid)
    {
        public override OTSLogicalAnd CreateInstance() => new(Name, LibraryGuid);
    }

    public class OTSLogicalAnd(string name, Guid libGuid) : OTSLogicalBase(name, libGuid, (OTSData? i1, OTSData? i2) 
        => (i1??new OTSData(OTSTypes.BOOL, false)).As<bool>() && (i2??new OTSData(OTSTypes.BOOL, false)).As<bool>())
    { }
    

    public class OTSLogicalOrTemplate(Guid libGuid) : OTSLogicalTemplate(StdLibUtils.LogicalOr, libGuid)
    {
        public override OTSLogicalOr CreateInstance() => new(Name, LibraryGuid);
    }

    public class OTSLogicalOr(string name, Guid libGuid) : OTSLogicalBase(name, libGuid, (OTSData? i1, OTSData? i2)
        => (i1??new OTSData(OTSTypes.BOOL, false)).As<bool>() || (i2??new OTSData(OTSTypes.BOOL, false)).As<bool>())
    { }


    public class OTSLogicalXorTemplate(Guid libGuid) : OTSLogicalTemplate(StdLibUtils.LogicalXor, libGuid)
    {
        public override OTSLogicalXor CreateInstance() => new(Name, LibraryGuid);
    }

    public class OTSLogicalXor(string name, Guid libGuid) : OTSLogicalBase(name, libGuid, (OTSData? i1, OTSData? i2) 
        => (i1??new OTSData(OTSTypes.BOOL, false)) ^ (i2??new OTSData(OTSTypes.BOOL, false)))
    { }


}
