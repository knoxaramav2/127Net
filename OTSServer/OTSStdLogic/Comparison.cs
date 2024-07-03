using OTSSDK;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTStdLogic
{

    #region EQUALITY

    public class OTSLogicalEqualTemplate(Guid libGuid) : 
        OTSLogicalTemplate(StdLibUtils.LogicalEqual, libGuid)
    {
        public override OTSLogicalEqual CreateInstance() => new(Name, LibraryGuid);
    }

    public class OTSLogicalEqual(string name, Guid libGuid) : 
        OTSLogicalBase(name, libGuid, (OTSData? i1, OTSData? i2) => i1 == i2)
    { }

    public class OTSLogicalNotEqualTemplate(Guid libGuid) : 
        OTSLogicalTemplate(StdLibUtils.LogicalNotEqual, libGuid)
    {
        public override OTSNotLogicalEqual CreateInstance() => new(Name, LibraryGuid);
    }

    public class OTSNotLogicalEqual(string name, Guid libGuid) : 
        OTSLogicalBase(name, libGuid, (OTSData? i1, OTSData? i2) => i1 != i2)
    { }

    #endregion

    #region LESS

    public class OTSLogicalLessTemplate(Guid libGuid) : 
        OTSLogicalTemplate(StdLibUtils.LogicalLess, libGuid)
    {
        public override OTSLogicalLess CreateInstance() => new(Name, LibraryGuid);
    }

    public class OTSLogicalLess(string name, Guid libGuid) : 
        OTSLogicalBase(name, libGuid, (OTSData? i1, OTSData? i2) => i1 < i2)
    { }

    public class OTSLogicalLessEqualTemplate(Guid libGuid) : 
        OTSLogicalTemplate(StdLibUtils.LogicalLessEqual, libGuid)
    {
        public override OTSLogicalLessEqual CreateInstance() => new(Name, LibraryGuid);
    }

    public class OTSLogicalLessEqual(string name, Guid libGuid) : 
        OTSLogicalBase(name, libGuid, (OTSData? i1, OTSData? i2) => i1 <= i2)
    { }

    #endregion

    #region GREATER

    public class OTSLogicalGreaterTemplate(Guid libGuid) : 
        OTSLogicalTemplate(StdLibUtils.LogicalGreater, libGuid)
    {
        public override OTSLogicalGreater CreateInstance() => new(Name, LibraryGuid);
    }

    public class OTSLogicalGreater(string name, Guid libGuid) : 
        OTSLogicalBase(name, libGuid, (OTSData? i1, OTSData? i2) => i1 > i2)
    { }

    public class OTSLogicalGreaterEqualTemplate(Guid libGuid) : 
        OTSLogicalTemplate(StdLibUtils.LogicalGreaterEqual, libGuid)
    {
        public override OTSLogicalGreaterEqual CreateInstance() => new(Name, LibraryGuid);
    }

    public class OTSLogicalGreaterEqual(string name, Guid libGuid) : 
        OTSLogicalBase(name, libGuid, (OTSData? i1, OTSData? i2) => i1 >= i2)
    { }

    #endregion
}
