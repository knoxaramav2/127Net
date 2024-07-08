using OTSSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OTSStdProvider
{
    public class OTSRandomProviderTemplate<T>(string name, Guid libGuid, string description, OTSTypes type, Func<T, T, IOTSData> proc) : 
        ProviderComponentTemplateBase(name, libGuid, description,
            [new OTSOutputTemplate("Value", type)], 
            [
                new OTSConfigFieldTemplate("MinValue", type, Guid.Empty, editLock: EditLock.Unlocked),
                new OTSConfigFieldTemplate("MaxValue", type, Guid.Empty, editLock: EditLock.Unlocked),
            ]
            )
    {
        public override OTSRandomProvider<T> CreateInstance()
        {
            return new(this, proc);
        }

    }

    public class OTSRandomProvider<T> : ProviderComponentBase
    {
        IOTSConfigField MinVal { get; }
        IOTSConfigField MaxVal { get; }
        IOTSOutput Output { get; }

        Func<T, T, IOTSData> UpdateProc { get; }

        public OTSRandomProvider(OTSRandomProviderTemplate<T> template, Func<T, T, IOTSData> proc) : 
            base(template)
        {
            MinVal = Fields.First(x => x.Name.Equals("MinValue", StringComparison.OrdinalIgnoreCase));
            MaxVal = Fields.First(x => x.Name.Equals("MaxValue", StringComparison.OrdinalIgnoreCase));
            Output = Outputs.First();
            UpdateProc = proc;
        }

        public override void Update()
        {
            Output.Value = UpdateProc.Invoke(
                MinVal.Get()!.As<T>()!, 
                MaxVal.Get()!.As<T>()!);
        }
    }

    public static class OTSRandomTemplates
    {
        public static OTSRandomProviderTemplate<long> Signed(Guid libGuid) =>
            new (StdLibUtils.ProvidersRandomSigned, libGuid, "Random signed value", OTSTypes.SIGNED,
                (long min, long max) => new OTSData(OTSTypes.SIGNED, new Random().NextInt64(min, max)));

        public static OTSRandomProviderTemplate<double> Decimal(Guid libGuid) =>
            new (StdLibUtils.ProvidersRandomSigned, libGuid, "Random floating point value", OTSTypes.SIGNED, 
                (double min, double max) => new OTSData(OTSTypes.DECIMAL, new Random().NextDouble() * (max-min) + min));
    }
}
