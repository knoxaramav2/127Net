using OTSCommon.Plugins;
using OTSExecution;
using OTSSDK;
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
        private OTSExecManager _runner;
        private SingleSetupPlugins _setupPlugins;

        [TearDown]
        public void Cleanup() { _setup.Dispose(); }

        [SetUp]
        public void Setup()
        {
            _setup = SingleSetup.GetInstance()
                .EnsurePlugins()
                .EnsureLibrary(StdLibUtils.ProvidersLibName)
                .EnsureComponent(StdLibUtils.ProvidersLibName, StdLibUtils.ProvidersConstBool)
                .EnsureComponent(StdLibUtils.ProvidersLibName, StdLibUtils.ProvidersConstSigned)

                .EnsureLibrary(StdLibUtils.MonitorLibName)
                .EnsureComponent(StdLibUtils.MonitorLibName, StdLibUtils.RawMonitor)

                .EnsureLibrary(StdLibUtils.LogicLibName)
                .EnsureComponent(StdLibUtils.LogicLibName, StdLibUtils.LogicalAnd)
                .EndPlugins;

            _setupPlugins = _setup.PluginSetups!;
            
            _runner = new OTSExecManager();

            Console.WriteLine(_runner.GetManifest());
        }

        [Test]
        public void BuildRunAndCircuit()
        {
            //_setup.PluginSetups?.GetComponent("provider", out var provider);
            //_setup.PluginSetups?.GetComponent("and", out var and);
            //_setup.PluginSetups?.GetComponent("monitor", out var monitor);
        }

        [Test]
        public void VerifyComponentTypes()
        {
            _setupPlugins.GetComponent(StdLibUtils.ProvidersConstBool, out var provider1);
            _setupPlugins.GetComponent(StdLibUtils.LogicalAnd, out var and);
            _setupPlugins.GetComponent(StdLibUtils.RawMonitor, out var monitor);

            Assert.Multiple(() =>
            {
                Assert.That(provider1?.ComponentClass.Value, Is.EqualTo(OTSComponentClass.PROVIDER));
                Assert.That(and?.ComponentClass.Value, Is.EqualTo(OTSComponentClass.ACTUATOR));
                Assert.That(monitor?.ComponentClass.Value, Is.EqualTo(OTSComponentClass.MONITOR));
            });
        }
    }
}
