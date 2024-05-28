using HomeCore.Data;
using HomeCore.Utility.Hardware;
using Microsoft.AspNetCore.Components;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Diagnostics;

namespace HomeCore.Utility
{
    public class SystemState
    {
        private static SystemState? _instance = null;

        public bool HasAdmin { get; private set; }
        public Device? Device { get; private set; }
        public bool IsInitialized => HasAdmin;

        public SystemState(HomeCoreDbCtx ctx) {
            var dInfo = DeviceInfo.GetDeviceInfo();
            foreach(var uac in ctx.UserAccounts)
            {
                Debug.WriteLine($"::: {uac.UserName} | {uac.MaxAuthority.AuthLevel} | {(uac.MaxAuthority.AuthLevel == 0 ? "X" : "-")}");
            }
            HasAdmin = ctx.UserAccounts.Any(x => x.MaxAuthority.AuthLevel == 0);
            Device = ctx.Devices.FirstOrDefault(x => x.HwId == dInfo.SerialNumber);
        }

        public static void Initialize(HomeCoreDbCtx ctx)
        {
            _instance = new SystemState(ctx);
        }

        public static SystemState GetInstance()
        {
            return _instance ?? throw new InvalidOperationException("SystemState uninitialized");
        }
    }
}
