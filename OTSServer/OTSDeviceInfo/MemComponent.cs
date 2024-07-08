using OTSSDK;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace OTSDeviceInfo
{


    #if OS_WINDOWS
    internal class MemoryInfo
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        class MEMORYSTATUSEX
        {
            public uint dwLength;
            public uint dwMemoryLoad;
            public ulong ullTotalPhys;
            public ulong ullAvailPhys;
            public ulong ullTotalPageFile;
            public ulong ullAvailPageFile;
            public ulong ullTotalVirtual;
            public ulong ullAvailVirtual;
            public ulong ullAvailExtendedVirtual;

            public MEMORYSTATUSEX()
            {
                dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));
            }
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
#pragma warning disable SYSLIB1054 // Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time
        private static extern bool GlobalMemoryStatusEx([In, Out] MEMORYSTATUSEX lpBuffer);
#pragma warning restore SYSLIB1054 // Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time

        public static void GetMemoryStatus(
            out ulong totalMemoryGB, 
            out ulong availableMemoryGB)
        {
            var memoryStatus = new MEMORYSTATUSEX();
            if (GlobalMemoryStatusEx(memoryStatus))
            {
                totalMemoryGB = memoryStatus.ullTotalPhys;
                availableMemoryGB = memoryStatus.ullAvailPhys;
            }
            else
            {
                totalMemoryGB = 0;
                availableMemoryGB = 0;
            }
        }
    }
    #elif OS_LINUX
    internal class MemoryInfo
    {
        public static void GetMemoryStatus(
            out ulong totalMemoryGB, 
            out ulong availableMemoryGB)
        {
            totalMemoryGB = 0;
            availableMemoryGB = 0;
        }
    }
    #endif

    public class MemComponentTemplate(Guid libGuid) :
         OTSProviderTemplate<MemComponent>(StdLibUtils.MemoryMonitor, libGuid, 
             "Provide system and process memory information",
             [
                ProcessMemoryOutput(),
                AvailableMemoryOutput(),
                SystemMemoryOutput()
             ],
             []
             )
    {

        private static MemMonitorOutputTemplate ProcessMemoryOutput()
        {
            return new MemMonitorOutputTemplate(
                StdLibUtils.ProcessMemory, OTSTypes.SIGNED
                );
        }

        private static MemMonitorOutputTemplate AvailableMemoryOutput()
        {
            return new MemMonitorOutputTemplate(
               StdLibUtils.SystemMemoryAvailable, OTSTypes.SIGNED
                );
        }

        private static MemMonitorOutputTemplate SystemMemoryOutput()
        {
            return new MemMonitorOutputTemplate(
                StdLibUtils.SystemMemoryTotal, OTSTypes.SIGNED
                );
        }

        public override MemComponent CreateInstance()
        {
            return new MemComponent(this);
        }
    }

    public class MemComponent : OTSProvider
    {
        private IOTSOutput ProcessOut { get; set; }
        private IOTSOutput AvailableOut { get; set; }
        private IOTSOutput TotalOut { get; set; }

        internal static Process _proc = Process.GetCurrentProcess();

        public MemComponent(MemComponentTemplate template) : 
            base(template)
        {
            ProcessOut = Outputs.First(x => x.Name.Equals(StdLibUtils.ProcessMemory, StringComparison.OrdinalIgnoreCase));
            AvailableOut = Outputs.First(x => x.Name.Equals(StdLibUtils.SystemMemoryAvailable, StringComparison.OrdinalIgnoreCase));
            TotalOut = Outputs.First(x => x.Name.Equals(StdLibUtils.SystemMemoryTotal, StringComparison.OrdinalIgnoreCase));
        }

        public override void Update()
        {
            ProcessOut.Value = new OTSData(OTSTypes.SIGNED, GetProcessMemory());
            AvailableOut.Value = new OTSData(OTSTypes.UNSIGNED, GetAvailableSystemMemory());
            TotalOut.Value = new OTSData(OTSTypes.UNSIGNED, GetTotalSystemMemory());
        }

        private static ulong GetAvailableSystemMemory()
        {
            if (OperatingSystem.IsWindows())
            {
                MemoryInfo.GetMemoryStatus(out var _, out var availableMemory);
                return availableMemory;
            }
            
            return 0;
        }

        private static ulong GetTotalSystemMemory()
        {
            if (OperatingSystem.IsWindows())
            {
                MemoryInfo.GetMemoryStatus(out var totalMem, out var _);
                return totalMem;
            }
            
            return 0;
        }

        private static long GetProcessMemory() => _proc.PrivateMemorySize64;
    }

    public class MemMonitorOutputTemplate(string name, OTSTypes type) : OTSOutputTemplate(name, type)
    {
    }
}
