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

    public class MemComponentTemplate(Guid libGuid) : OTSComponentTemplate<IOTSComponent>("MemoryMonitor", libGuid, false)
    {
        public override MemComponent CreateInstance() => new (Name, LibraryGuid);
        
    }

    public class MemComponent : OTSComponent
    {
        internal static Process _proc = Process.GetCurrentProcess();

        public MemComponent(string name, Guid libGuid) : base(name, libGuid)
        {
            Outputs = [
                new ProcessMemAllocated(ID, _proc),
                new SystemMemoryAvailable(ID),
                new SystemMemoryTotal(ID)
                ];  
        }
    }

    public class ProcessMemAllocated(Guid componentId, Process proc) : OTSOutput("ProcessMemory", componentId, OTSTypes.SIGNED)
    {
        public override IOTSData? Get()
        {
            return new OTSData(OTSTypes.SIGNED, proc.PrivateMemorySize64);
        }
    }

    public class SystemMemoryAvailable(Guid componentId) : OTSOutput("SystemMemoryAvailable", componentId, OTSTypes.UNSIGNED)
    {
        public override IOTSData? Get() => new OTSData(OTSTypes.UNSIGNED, GetAvailableSystemMemory());

        private static ulong GetAvailableSystemMemory()
        {
            if (OperatingSystem.IsWindows())
            {
                MemoryInfo.GetMemoryStatus(out var _, out var availableMemory);
                return availableMemory;
            }
            
            return 0;
        }
    }

    public class SystemMemoryTotal(Guid componentId) : OTSOutput("SystemMemoryTotal", componentId, OTSTypes.UNSIGNED)
    {

        public override IOTSData? Get() => new OTSData(OTSTypes.UNSIGNED, GetTotalSystemMemory());

        private static ulong GetTotalSystemMemory()
        {
            if (OperatingSystem.IsWindows())
            {
                MemoryInfo.GetMemoryStatus(out var totalMem, out var _);
                return totalMem;
            }
            
            return 0;
        }
    }
}
