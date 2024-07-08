using OTSSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTSStdProvider
{
    public class ProviderComponentTemplateBase(string name, Guid libGuid,
        string description,
        IEnumerable<IOTSOutputTemplate> outputs,
        List<IOTSConfigFieldTemplate> fields
        ) : 
        OTSProviderTemplate<ProviderComponentBase>(name, libGuid, description,
            outputs, fields)
    {

    }

    public class ProviderComponentBase(ProviderComponentTemplateBase template) : OTSProvider(template)
    {
    }
}
