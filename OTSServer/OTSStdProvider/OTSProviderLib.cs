using OTSSDK;
using System.Runtime.InteropServices;

namespace OTSStdProvider
{
    public class OTSProviderLib : IOTSLibrary
    {
        //TODO Replace fields with CICD configurable
        public string Name { get; } = StdLibUtils.ProvidersLibName;
        public string Version { get; } = "0.1.0";
        public string Author { get; } = "KnoxaramaV2";
        public string Platform { get; } = CommonUtil.GetOsStr; 
        public string Repository { get; } = "https://github.com/knoxaramav2/127Net";
        public string Description { get; } = "Generated value nodes.";
        public Guid ID { get; } = Guid.NewGuid();
        public IEnumerable<IOTSComponentTemplate<IOTSComponent>> Components { get; }
        public IOTSComponent? GetComponent(string name) 
            => Components.FirstOrDefault(x => name.Equals(x.Name, StringComparison.OrdinalIgnoreCase))?.CreateInstance();

        public OTSProviderLib()
        {
            Components = [
                new OTSConstantProviderTemplate(StdLibUtils.ProvidersConstSigned, ID, OTSTypes.SIGNED),
                new OTSConstantProviderTemplate(StdLibUtils.ProvidersConstUnsigned, ID, OTSTypes.UNSIGNED),
                new OTSConstantProviderTemplate(StdLibUtils.ProvidersConstDecimal, ID, OTSTypes.DECIMAL),
                new OTSConstantProviderTemplate(StdLibUtils.ProvidersConstBool, ID, OTSTypes.BOOL),
                new OTSConstantProviderTemplate(StdLibUtils.ProvidersConstString, ID, OTSTypes.STRING),
                new OTSConstantProviderTemplate(StdLibUtils.ProvidersConstMap, ID, OTSTypes.MAP),
                new OTSConstantProviderTemplate(StdLibUtils.ProvidersConstList, ID, OTSTypes.LIST),

                new OTSRandomProviderTemplate<long>(StdLibUtils.ProvidersRandomSigned, ID, OTSTypes.SIGNED, (long min, long max) 
                    => (new OTSData(OTSTypes.SIGNED, new Random().NextInt64(min, max)))),
                new OTSRandomProviderTemplate<double>(StdLibUtils.ProvidersRandomDecimal, ID, OTSTypes.DECIMAL, (double min, double max) 
                    => (new OTSData(OTSTypes.DECIMAL, (new Random()).NextDouble() * (max-min) + min 
                    ))),
            ];
        }
    }
}
