using OTSSDK;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace OTSStdMath
{
    public class OTSMathAdditionTemplate(Guid libGuid) : OTSMathTemplate(StdLibUtils.MathAddition, libGuid)
    {
        public override OTSMathAddition CreateInstance() => new(Name, LibraryGuid);
    }

    public class OTSMathAddition(string name, Guid libGuid) : OTSMathBase(name, libGuid, (OTSData? i1, OTSData? i2) 
        => (i1??new OTSData()) + (i2??new OTSData()))
    { }

    public class OTSMathSubtractionTemplate(Guid libGuid) : OTSMathTemplate(StdLibUtils.MathSubtraction, libGuid)
    {
        public override OTSMathSubtraction CreateInstance() => new(Name, LibraryGuid);
    }

    public class OTSMathSubtraction(string name, Guid libGuid) : OTSMathBase(name, libGuid, (OTSData? i1, OTSData? i2) 
        => (i1??new OTSData()) - (i2??new OTSData()))
    { }

    public class OTSMathMultiplicationTemplate(Guid libGuid) : OTSMathTemplate(StdLibUtils.MathMultiplication, libGuid)
    {
        public override OTSMathMultiplication CreateInstance() => new(Name, LibraryGuid);
    }

    public class OTSMathMultiplication(string name, Guid libGuid) : OTSMathBase(name, libGuid, (OTSData? i1, OTSData? i2) 
        => (i1??new OTSData()) * (i2??new OTSData()))
    { }

    public class OTSMathDivisionTemplate(Guid libGuid) : OTSMathTemplate(StdLibUtils.MathDivision, libGuid)
    {
        public override OTSMathDivision CreateInstance() => new(Name, LibraryGuid);
    }

    public class OTSMathDivision(string name, Guid libGuid) : OTSMathBase(name, libGuid, (OTSData? i1, OTSData? i2) 
        => (i1??new OTSData()) / (i2??new OTSData()))
    { }
}
