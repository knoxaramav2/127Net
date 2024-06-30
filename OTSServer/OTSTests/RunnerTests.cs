using OTSCommon.Plugins;
using OTSExecution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTSTests
{
    internal class RunnerTests
    {
        private SingleSetup _setup;
        private IOTSExecManager _runner;

        [TearDown]
        public void Cleanup() { _setup.Dispose(); }

        [SetUp]
        public void Setup()
        {
            _setup = SingleSetup.GetInstance()
                .EnsurePlugins()
                .EnsureLibrary("OTSProvider", "providers")
                .EnsureComponent("SignedProvider", "provider")

                .EnsureLibrary("OTSMonitor", "monitors")
                .EnsureComponent("RawMonitor", "monitor")

                .EnsureLibrary("OTSLogic", "logic")
                .EnsureComponent("LogicalAnd", "and")
                .EnsureComponent("LogicalAnd", "or")
                .EnsureComponent("LogicalAnd", "xor")
                .EndPlugins;
            
            _runner = new OTSExecManager();

            Console.WriteLine(_runner.GetManifest());
        }

        [Test]
        public void BuildRunAndCircuit()
        {
            _setup.PluginSetups?.GetComponent("provider", out var provider);
            _setup.PluginSetups?.GetComponent("and", out var and);
            _setup.PluginSetups?.GetComponent("monitor", out var monitor);

            
        }

    }
}
