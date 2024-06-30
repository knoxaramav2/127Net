﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTSSDK
{
    public enum OTSTypes
    {
        NONE, 
        BOOL,
        SIGNED, UNSIGNED,
        DECIMAL,
        STRING,
        LIST, MAP
    }

    public static class OTSTypesUtil
    {
        public static OTSTypes MapValueToOTSType(object? value) =>
            value switch{
                long or int or short => OTSTypes.SIGNED,
                ulong or uint or short or byte => OTSTypes.UNSIGNED,
                float or double or decimal => OTSTypes.DECIMAL,
                bool => OTSTypes.BOOL,
                string => OTSTypes.STRING,
                List<object> => OTSTypes.LIST,
                Dictionary<string, object> => OTSTypes.MAP,
                _ => OTSTypes.NONE,
            };
    }
}
