using OTSSDK;

namespace OTSStdMonitor
{
    public class OTSMonitorLib : IOTSLibrary
    {
        //TODO Replace fields with CICD configurable
        public string Name { get; } = StdLibUtils.MonitorLibName;
        public string Version { get; } = "0.1.0";
        public string Author { get; } = "KnoxaramaV2";
        public string Platform { get; } = CommonUtil.GetOsStr; 
        public string Repository { get; } = "https://github.com/knoxaramav2/127Net";
        public string Description { get; } = "Result monitors.";
        public Guid ID { get; } = Guid.NewGuid();
        public IEnumerable<IOTSComponentTemplate<IOTSComponent>> Components { get; }
        public IOTSComponent? GetComponent(string name) 
            => Components.FirstOrDefault(x => name.Equals(x.Name, StringComparison.OrdinalIgnoreCase))?.CreateInstance();

        public OTSMonitorLib()
        {
            Components = [
                new RawMonitorTemplate(ID)
            ];
        }
    }
}
