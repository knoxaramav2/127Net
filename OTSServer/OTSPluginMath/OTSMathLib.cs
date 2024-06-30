using OTSSDK;
using OTSStdMath;
using OTSStdMathx;

namespace OTSPluginMath
{
    public class OTSMathLib : IOTSLibrary
    {
        //TODO Replace fields with CICD configurable
        public string Name { get; } = "OTSMath";
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
            Components = [
                new AdditionComponentTemplate(ID),
                new SubtractionComponentTemplate(ID)
            ];
        }
    }
}
