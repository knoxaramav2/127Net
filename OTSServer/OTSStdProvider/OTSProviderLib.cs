﻿using OTSSDK;
using System.Runtime.InteropServices;

namespace OTSStdProvider
{
    public class OTSProviderLib : IOTSLibrary
    {
        //TODO Replace fields with CICD configurable
        public string Name { get; } = "OTSProvider";
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
                new SignedConstantTemplate(ID),
                new UnsignedConstantTemplate(ID),
                new DecimalConstantTemplate(ID),
                new BoolConstantTemplate(ID),
                new StringConstantTemplate(ID),
                new ListConstantTemplate(ID),
                new MapConstantTemplate(ID),

                new SignedRandomTemplate(ID),
                new DecimalRandomTemplate(ID),
            ];
        }
    }
}
