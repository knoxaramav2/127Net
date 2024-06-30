using OTSCommon.Plugins;
using OTSSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTSTests.StdLibTests.Device
{
    internal class DeviceTests
    {
        private SingleSetup _setup;
        private PluginManager _pluginManager;
        private IOTSLibrary _deviceHwLib;

        [TearDown]
        public void Cleanup() { _setup.Dispose(); }

        [SetUp]
        public void Setup()
        {
            _setup = SingleSetup.GetInstance();
            _pluginManager = new();
            try
            {
                _deviceHwLib = _pluginManager.GetLibrary("OTSDeviceHwInfo")!;
            }
            catch (Exception) { Assert.Fail(); }

        }

        [Test]
        public void ProcessMemory()
        {
            var comp = _deviceHwLib.GetComponent("MemoryMonitor");

            Assert.Multiple(() =>
            {
                Assert.That(comp, Is.Not.Null);
                var processMem = comp!.GetOutput("ProcessMemory");
                Assert.That(processMem, Is.Not.Null);
                var value = processMem!.Get()?.As<long>();
                Assert.That(value, Is.Not.Zero);
                Console.WriteLine($"Process Memory: {value / (1024 * 1024)} MB");
            });
        }

        [Test]
        public void SystemMemory()
        {
            var comp = _deviceHwLib.GetComponent("MemoryMonitor");

            Assert.Multiple(() =>
            {
                Assert.That(comp, Is.Not.Null);
                var systemMemTotal = comp!.GetOutput("SystemMemoryTotal");
                var systemMemAvailable = comp!.GetOutput("SystemMemoryAvailable");

                Assert.That(systemMemTotal, Is.Not.Null);
                Assert.That(systemMemAvailable, Is.Not.Null);

                var totalMem = systemMemTotal!.Get()?.As<ulong>();
                var availMem = systemMemAvailable!.Get()?.As<ulong>();

                Assert.That(totalMem, Is.Not.Zero);
                Assert.That(availMem, Is.Not.Zero);

                const ulong GB = 1024 * 1024 * 1024;
                Console.WriteLine($"Total System Memory: {totalMem / GB} GB {totalMem % GB} MB");
                Console.WriteLine($"Available System Memory: {availMem / GB} GB {availMem % GB} MB");
            });
        }
    }
}
