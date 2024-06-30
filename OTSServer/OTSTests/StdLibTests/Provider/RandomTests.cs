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
        private IOTSLibrary _providerLib;
        private IOTSComponent _signedProvider;
        private IOTSComponent _doubleProvider;

        [TearDown]
        public void Cleanup() { _setup.Dispose(); }

        [SetUp]
        public void Setup()
        {
            _setup = SingleSetup.GetInstance();
            _pluginManager = new();
            try
            {
                _providerLib = _pluginManager.GetLibrary("OTSProvider")!;
                _signedProvider = _providerLib.GetComponent("SignedRandomProvider")!;
                _doubleProvider = _providerLib.GetComponent("DecimalRandomProvider")!;
            }
            catch (Exception) { Assert.Fail(); }
        }

        [Test]
        public void SignedRandom()
        {
            var minField = _signedProvider.GetConfig("MinValue");
            var maxField = _signedProvider.GetConfig("MaxValue");
            var result = _signedProvider.GetOutput("Result");

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(minField, Is.Not.Null);
                Assert.That(maxField, Is.Not.Null);
            });

            List<long> numbers = [];
            const long minVal = -200;
            const long maxVal = 200;
            minField.Set(new OTSData(OTSTypes.SIGNED,minVal));
            maxField.Set(new OTSData(OTSTypes.SIGNED, maxVal));

            for(var i = 0; i < 100; i++)
            {
                numbers.Add(result.Get()!.As<long>());
            }

            Assert.Multiple(() =>
            {
                Assert.That(numbers.Any(x => x < minVal || x > maxVal), Is.False);
                Assert.That(numbers.Distinct().ToList(), Has.Count.GreaterThan(1));
            });
        }

        [Test]
        public void DoubleRandom()
        {
            var minField = _doubleProvider.GetConfig("MinValue");
            var maxField = _doubleProvider.GetConfig("MaxValue");
            var result = _doubleProvider.GetOutput("Result");

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(minField, Is.Not.Null);
                Assert.That(maxField, Is.Not.Null);
            });

            List<double> numbers = [];
            const double minVal = -200.5;
            const double maxVal = 200.5;
            minField.Set(new OTSData(OTSTypes.DECIMAL, minVal));
            maxField.Set(new OTSData(OTSTypes.DECIMAL, maxVal));

            const uint numItems = 100;
            for(var i = 0; i < numItems; i++)
            {
                numbers.Add(result.Get()!.As<double>());
            }

            Assert.Multiple(() =>
            {
                Assert.That(numbers.Any(x => x < minVal || x > maxVal), Is.False);
                Assert.That(numbers.Distinct().ToList(), Has.Count.EqualTo(numItems));
            });
        }

    }
}
