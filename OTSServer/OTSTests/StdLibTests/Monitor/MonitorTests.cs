using OTSCommon.Plugins;
using OTSSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTSTests.StdLibTests.Monitor
{
    internal class MonitorTests
    {
        private SingleSetup _setup;
        private PluginManager _pluginManager;
        private IOTSLibrary _providerLib;
        private IOTSComponent _rawMonitor;
        private IOTSComponent _objectStore;

        [TearDown]
        public void Cleanup() { _setup.Dispose(); }

        [SetUp]
        public void Setup()
        {
            _setup = SingleSetup.GetInstance();
            _pluginManager = new();
            try
            {
                _providerLib = _pluginManager.GetLibrary("OTSMonitor")!;
                _rawMonitor = _providerLib.GetComponent("RawMonitor")!;
            }
            catch (Exception) { Assert.Fail(); }
        }


        [Test]
        public void AddAndVerifyMonitorHookup()
        {
            var nameStrIn = "StrInput";
            var nameLonIn = "LonInput";

            var monitor = _providerLib.GetComponent("RawMonitor");
            var constSignedView = _providerLib.GetComponent("SignedProvider");

            

            var output = constSignedView?.GetOutput("Result");
            //constSignedView.ConnectTo(output, monitor);

            var strIn = _rawMonitor.GetInput(nameStrIn);
            var lonIn = _rawMonitor.GetInput(nameLonIn);

            Assert.Multiple(() =>
            {
                Assert.That(strIn, Is.Not.Null);
                Assert.That(lonIn, Is.Not.Null);
            });
        }
    }
}
