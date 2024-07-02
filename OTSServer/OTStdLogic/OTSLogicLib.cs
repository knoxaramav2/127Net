﻿using OTSSDK;

namespace OTStdLogic
{
    public class OTSLogicLib : IOTSLibrary
    {
        //TODO Replace fields with CICD configurable
        public string Name { get; } = StdLibUtils.LogicLibName;
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
                new OTSLogicalAndTemplate(ID),
                new OTSLogicalOrTemplate(ID),
                new OTSLogicalXorTemplate(ID),

                new OTSLogicalEqualTemplate(ID),
                new OTSLogicalNotEqualTemplate(ID),
                new OTSLogicalLessTemplate(ID),
                new OTSLogicalLessEqualTemplate(ID),
                new OTSLogicalGreaterTemplate(ID),
                new OTSLogicalGreaterEqualTemplate(ID),

            ];
        }
    }
}
