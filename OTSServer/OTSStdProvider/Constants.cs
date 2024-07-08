using OTSSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;

namespace OTSStdProvider
{
    public class OTSConstantProviderTemplate(string name, Guid libGuid, string description, OTSTypes type) : 
        ProviderComponentTemplateBase(name, libGuid, description,
            [new OTSOutputTemplate("Value", type)], 
            [new OTSConfigFieldTemplate("Value", type, Guid.Empty, editLock: EditLock.Unlocked)]
            )
    {
        public override OTSConstantProvider CreateInstance()
        {
            return new OTSConstantProvider(this);
        }
    }

    public class OTSConstantProvider : ProviderComponentBase
    {
        IOTSConfigField ConfigValue { get; }
        IOTSOutput Output { get; }

        public OTSConstantProvider(OTSConstantProviderTemplate template) : 
            base(template)
        {
            ConfigValue = Fields.First();
            Output = Outputs.First();
        }

        public override void Update()
        {
            Output.Value = ConfigValue.Get();
        }
    }

    public static class OTSConstantTemplates
    {
        public static OTSConstantProviderTemplate Signed(Guid libGuid) =>
            new (StdLibUtils.ProvidersConstSigned, libGuid, "Constant signed value", OTSTypes.SIGNED);
        public static OTSConstantProviderTemplate Unsigned(Guid libGuid) =>
            new (StdLibUtils.ProvidersConstUnsigned, libGuid, "Constant unsigned value", OTSTypes.UNSIGNED);
        public static OTSConstantProviderTemplate Decimal(Guid libGuid) =>
            new (StdLibUtils.ProvidersConstDecimal, libGuid, "Constant floating point value", OTSTypes.DECIMAL);
        public static OTSConstantProviderTemplate Bool(Guid libGuid) =>
            new (StdLibUtils.ProvidersConstBool, libGuid, "Constant bool value", OTSTypes.BOOL);
        public static OTSConstantProviderTemplate String(Guid libGuid) =>
            new (StdLibUtils.ProvidersConstString, libGuid, "Constant string value", OTSTypes.STRING);
        public static OTSConstantProviderTemplate Map(Guid libGuid) =>
            new (StdLibUtils.ProvidersConstMap, libGuid, "Constant mapped values", OTSTypes.MAP);
        public static OTSConstantProviderTemplate List(Guid libGuid) =>
            new (StdLibUtils.ProvidersConstList, libGuid, "Constant list of values", OTSTypes.LIST);

    }
}
