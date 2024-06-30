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
    }
}
