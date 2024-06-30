using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTSSDK
{
    public enum OTSInfoType
    {
        Error, Warning, Info
    }

    public struct OTSInfoMessage(string message, OTSInfoType type)
    {
        public OTSInfoType Type { get; } = type;
        public string Message { get; } = message;
    }
}
