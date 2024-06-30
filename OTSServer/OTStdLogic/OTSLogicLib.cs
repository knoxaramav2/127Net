using OTSSDK;

namespace OTStdLogic
{
    public class OTSLogicLib : IOTSLibrary
    {
        //TODO Replace fields with CICD configurable
        public string Name { get; } = "OTSLogic";
        public string Version { get; } = "0.1.0";
        public string Author { get; } = "KnoxaramaV2";
        public string Platform { get; } = CommonUtil.GetOsStr; 
        public string Repository { get; } = "https://github.com/knoxaramav2/127Net";
        public string Description { get; } = "Logical nodes.";
        public Guid ID { get; } = Guid.NewGuid();
        public IEnumerable<IOTSComponentTemplate<IOTSComponent>> Components { get; }
        public IOTSComponent? GetComponent(string name) 
            => Components.FirstOrDefault(x => name.Equals(x.Name, StringComparison.OrdinalIgnoreCase))?.CreateInstance();

        public OTSLogicLib()
        {
            Components = [
                new LogicAndTemplate(ID),
                new LogicOrTemplate(ID),
                new LogicXORTemplate(ID),

                new LogicEqualTemplate(ID),
                new LogicNotEqualTemplate(ID),
                new LogicGreaterTemplate(ID),
                new LogicLessTemplate(ID),
                new LogicGreaterEqualTemplate(ID),
                new LogicLessEqualTemplate(ID),
            ];
        }
    }
}
