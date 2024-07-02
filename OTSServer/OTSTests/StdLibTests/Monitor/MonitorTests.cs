using Microsoft.Extensions.Options;
using OTSRunner;
using OTSSDK;
using System.Diagnostics.CodeAnalysis;

namespace OTSTests.StdLibTests.Monitor
{
    internal class MonitorTests
    {
        private SingleSetup _setup;
        private SingleSetupPlugins _setupPlugins;
        private OTSObjectManager _manager;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private IOTSComponent _monitor;
        private IOTSComponent _boolProvider;
        private IOTSComponent _signedProvider;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.


        [TearDown]
        public void Cleanup() { _setup.Dispose(); }

        [SetUp]
        [MemberNotNull(nameof(_monitor))]
        [MemberNotNull(nameof(_boolProvider))]
        [MemberNotNull(nameof(_signedProvider))]
        public void Setup()
        {
            _manager = new OTSObjectManager();
            _setup = SingleSetup.GetInstance()
                .EnsurePlugins()
                .EnsureLibrary(StdLibUtils.ProvidersLibName)
                .EnsureComponent(StdLibUtils.ProvidersLibName, StdLibUtils.ProvidersConstBool)
                .EnsureComponent(StdLibUtils.ProvidersLibName, StdLibUtils.ProvidersConstSigned)

                .EnsureLibrary(StdLibUtils.MonitorLibName)
                .EnsureComponent(StdLibUtils.MonitorLibName, StdLibUtils.RawMonitor)
                .EndPlugins;

            _setup.PluginSetups!.GetComponent(StdLibUtils.RawMonitor, out _monitor!);
            _setup.PluginSetups!.GetComponent(StdLibUtils.ProvidersConstBool, out _boolProvider!);
            _setup.PluginSetups!.GetComponent(StdLibUtils.ProvidersConstSigned, out _signedProvider!);

            _manager.AddComponent(_monitor);
            _manager.AddComponent(_boolProvider);
            _manager.AddComponent(_signedProvider);
            _manager.LinkComponent(_boolProvider.GetOutput("Result")!, _monitor);
            _manager.LinkComponent(_signedProvider.GetOutput("Result")!, _monitor);
            _manager.BuildLinkOrder();

            _setupPlugins = _setup.PluginSetups!;
        }

        public bool SetAndReturnBoolMonitorValue(bool value)
        {
            _boolProvider.GetConfig("Value")!.Set(new OTSData(OTSTypes.BOOL, value));
            _manager.PropgateSignals();
            var resultConfig = _monitor.GetConfig("View 1")!;
            var result = resultConfig.Get()!.As<bool>();
            return result;
        }

        [Test]
        public void BooMonitor1()
        {
            var res = SetAndReturnBoolMonitorValue(true);
            Assert.That(res, Is.True);
        }

        [Test]
        public void BooMonitor0()
        {
            var res = SetAndReturnBoolMonitorValue(false);
            Assert.That(res, Is.False);
        }
    }
}
