using OTSSDK;
using OTSStdMath;

namespace OTSPluginMath
{
    public class OTSMathLib : IOTSLibrary
    {
        //TODO Replace fields with CICD configurable
        public string Name { get; } = StdLibUtils.MathLibName;
        public string Version { get; } = "0.1.0";
        public string Author { get; } = "KnoxaramaV2";
        public string Platform { get; } = CommonUtil.GetOsStr; 
        public string Repository { get; } = "https://github.com/knoxaramav2/127Net";
        public string Description { get; } = "Basic math nodes.";
        public Guid ID { get; } = Guid.NewGuid();
        public IEnumerable<IOTSComponentTemplate<IOTSComponent>> Components { get; }
        public IOTSComponent? GetComponent(string name) 
            => Components.FirstOrDefault(x => name.Equals(x.Name, StringComparison.OrdinalIgnoreCase))?.CreateInstance();

        public OTSMathLib()
        {
            var addition = new OTSMathBinaryTemplate(StdLibUtils.MathAddition, ID, "Add two numbers", 
                (OTSData? i1, OTSData? i2) 
                => (i1??new OTSData()) + (i2??new OTSData()));
            var subtraction = new OTSMathBinaryTemplate(StdLibUtils.MathSubtraction, ID, "Subtract two numbers", 
                (OTSData? i1, OTSData? i2) 
                => (i1??new OTSData()) - (i2??new OTSData()));
            var multiply = new OTSMathBinaryTemplate(StdLibUtils.MathMultiplication, ID,"Multiply two numbers",  
                (OTSData? i1, OTSData? i2) 
                => (i1??new OTSData()) * (i2??new OTSData()));
            var divide = new OTSMathBinaryTemplate(StdLibUtils.MathDivision, ID, "Divide two numbers", 
                (OTSData? i1, OTSData? i2) 
                => (i1??new OTSData()) / (i2??new OTSData()));

            Components = [
                addition, subtraction, multiply, divide
            ];
        }
    }
}
