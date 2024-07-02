using OTSSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OTSStdProvider
{
    public class OTSRandomProviderTemplate<T>(string name, Guid libGuid, OTSTypes type, Func<T, T, IOTSData> randomFunc) : 
        OTSProviderTemplate(name, libGuid)
        where T : INumber<T>
    {
        public override IOTSComponent CreateInstance()
        {
            return new OTSRandomProvider<T>(Name, LibraryGuid, type, randomFunc);
        }
    }

    public class OTSRandomProvider<T> : OTSProviderBase
        where T : INumber<T>
    {
        protected IOTSConfigField MinConfig { get; set; }
        protected IOTSConfigField MaxConfig { get; set; }
        private Func<T, T, IOTSData> UpdateFunc { get; set; }

        public OTSRandomProvider(string name, Guid libGuid, OTSTypes type, Func<T, T, IOTSData> updateFunc) :
            base(name, libGuid, type)
        {
            MinConfig = new OTSConfigField("MinValue", type);
            MaxConfig = new OTSConfigField("MaxValue", type);

            Fields = [MinConfig, MaxConfig];

            UpdateFunc = updateFunc;
        }

        public override void Update()
        {
            var minVal = TypeConversion.EnsureValue(MinConfig.Get(), OTSType).As<T>();
            var maxVal = TypeConversion.EnsureValue(MaxConfig.Get(), OTSType).As<T>();
            Output.Value = UpdateFunc(minVal!, maxVal!);
        }
    }

    #region SIGNED RANDOM
    #endregion
}
