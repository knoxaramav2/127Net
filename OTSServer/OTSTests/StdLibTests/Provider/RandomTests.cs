using OTSCommon.Plugins;
using OTSSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZstdSharp.Unsafe;

namespace OTSTests.StdLibTests.Provider
{
    internal class RandomTests
    {
        private SingleSetup _setup;
        private PluginManager _pluginManager;
        private SingleSetupPlugins _setupPlugins;

        [TearDown]
        public void Cleanup() { _setup.Dispose(); }

        [SetUp]
        public void Setup()
        {
            _pluginManager = new();
            _setup = SingleSetup.GetInstance()
                .EnsurePlugins()
                .EnsureLibrary(StdLibUtils.ProvidersLibName)
                .EnsureComponent(StdLibUtils.ProvidersLibName, StdLibUtils.ProvidersRandomSigned)
                .EnsureComponent(StdLibUtils.ProvidersLibName, StdLibUtils.ProvidersRandomDecimal)
                .EndPlugins;
            _setupPlugins =  _setup.PluginSetups!;
        }

        private T? SetTestValue<T>(string componentName, T min, T max)
        {
            var type = TypeConversion.TypeFromGeneric<T>();
            _setupPlugins.GetComponent(componentName, out var cfg);
            cfg!.GetConfig("MinValue")?.Set(new OTSData(type, min));
            cfg!.GetConfig("MaxValue")?.Set(new OTSData(type, max));
            cfg.Update();
            var res = cfg.GetOutput("Result")!.Value!.As<T>();

            return res;
        }

        [Test]
        public void SignedRandom()
        {
            const long min = -1000;
            const long max = 1000;

            List<long> values = [];
            for(var i = 0; i < 20; i++)
            {
                var value = SetTestValue<long>(StdLibUtils.ProvidersRandomSigned, min, max);
                values.Add(value);
            }

            Assert.Multiple(() =>
            {
                var allBetween = values.All(x => x >= min && x < max);
                Assert.That(allBetween, Is.True);

                var distinctCount = values.Distinct().Count();
                Assert.That(distinctCount, Is.GreaterThan(1));
            });
        }

        [Test]
        public void DoubleRandom()
        {
            const double min = -1000.5;
            const double max = 1000.8;

            List<double> values = [];
            for(var i = 0; i < 20; i++)
            {
                var value = SetTestValue<double>(StdLibUtils.ProvidersRandomSigned, min, max);
                values.Add(value);
            }

            Assert.Multiple(() =>
            {
                var allBetween = values.All(x => x >= min && x < max);
                Assert.That(allBetween, Is.True);

                var distinctCount = values.Distinct().Count();
                Assert.That(distinctCount, Is.GreaterThan(1));
            });
        }

    }
}
