using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OTSSDK
{
    public static class CommonUtil
    {
        public static string GetOsStr =>
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "OS_WIN" :
            RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? "OS_LINUX" :
            RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? "OS_OSX" :
            RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD) ? "OS_FBSD" :
            string.Empty;

    public static OTSComponentClass IdentifyComponentClass(
        IOTSComponentDefinition<IOTSInputDefinition, IOTSViewDefinition, IOTSOutputDefinition, IOTSConfigFieldDefinition>
        definition)
    {
        var bin = (definition.Inputs.Any(), definition.Outputs.Any(), definition.AllowExpansion);

        return bin switch
        {
            (false, false, true) or (true, false, false) or (true, false, true) => OTSComponentClass.MONITOR,
            (true, true, false) or (true, true, true) => OTSComponentClass.ACTUATOR,
            (false, false, false) => OTSComponentClass.SINGLE,
            (false, true, true) => OTSComponentClass.NOMAD,
            (false, true, false) => OTSComponentClass.PROVIDER
        };
    }

        internal static OTSComponentClass IdentifyComponentClass<TInput, TView, TOutput, TField>(OTSComponentBase<TInput, TView, TOutput, TField> definition)
            where TInput : IOTSInputDefinition
            where TView : IOTSViewDefinition
            where TOutput : IOTSOutputDefinition
            where TField : IOTSConfigFieldDefinition
        {
            var bin = (definition.Inputs.Any(), definition.Outputs.Any(), definition.AllowExpansion);

            return bin switch
            {
                (false, false, true) or (true, false, false) or (true, false, true) => OTSComponentClass.MONITOR,
                (true, true, false) or (true, true, true) => OTSComponentClass.ACTUATOR,
                (false, false, false) => OTSComponentClass.SINGLE,
                (false, true, true) => OTSComponentClass.NOMAD,
                (false, true, false) => OTSComponentClass.PROVIDER
            };
        }
    }


}
