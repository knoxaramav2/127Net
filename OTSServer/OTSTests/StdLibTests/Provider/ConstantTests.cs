using Google.Protobuf.WellKnownTypes;
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
    internal class ConstantTests
    {
        private SingleSetup _setup;
        private PluginManager _pluginManager;
        private IOTSLibrary _providerLib;
        private IOTSComponent _signedProvider;
        private IOTSComponent _unsignedProvider;
        private IOTSComponent _decimalProvider;
        private IOTSComponent _boolProvider;
        private IOTSComponent _stringProvider;
        private IOTSComponent _listProvider;
        private IOTSComponent _mapProvider;

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
                _signedProvider = _providerLib.GetComponent("SignedProvider")!;
                _unsignedProvider = _providerLib.GetComponent("UnsignedProvider")!;
                _decimalProvider = _providerLib.GetComponent("DecimalProvider")!;
                _boolProvider = _providerLib.GetComponent("BoolProvider")!;
                _stringProvider = _providerLib.GetComponent("StringProvider")!;
                _listProvider = _providerLib.GetComponent("ListProvider")!;
                _mapProvider = _providerLib.GetComponent("MapProvider")!;
            }
            catch (Exception) { Assert.Fail(); }
        }

        [Test]
        public void SignedConst()
        {
            const long testVal = -5284;
            var cfg = _signedProvider.GetConfig("Value");
            Assert.That(cfg, Is.Not.Null);
            cfg.Set(new OTSData(OTSTypes.SIGNED, testVal));
            Assert.That(cfg.Get()!.As<long>(), Is.EqualTo(testVal));
        }

        [Test]
        public void UnsignedConst()
        {
            const ulong testVal = 20050;
            var cfg = _unsignedProvider.GetConfig("Value");
            Assert.That(cfg, Is.Not.Null);
            cfg.Set(new OTSData(OTSTypes.UNSIGNED, testVal));
            Assert.That(cfg.Get()!.As<long>(), Is.EqualTo(testVal));
        }

        [Test]
        public void DoubleConst()
        {
            const double testVal = 200.54f;
            var cfg = _decimalProvider.GetConfig("Value");
            Assert.That(cfg, Is.Not.Null);
            cfg.Set(new OTSData(OTSTypes.DECIMAL, testVal));
            Assert.That(cfg.Get()!.As<double>(), Is.EqualTo(testVal));
        }

        [Test]
        public void BoolConst()
        {
            const bool testVal = true;
            var cfg = _boolProvider.GetConfig("Value");
            Assert.That(cfg, Is.Not.Null);
            cfg.Set(new OTSData(OTSTypes.BOOL, testVal));
            Assert.That(cfg.Get()!.As<bool>(), Is.EqualTo(testVal));
        }

        [Test]
        public void StringConst()
        {
            const string testVal = "ThisIs A test String!";
            var cfg = _stringProvider.GetConfig("Value");
            Assert.That(cfg, Is.Not.Null);
            cfg.Set(new OTSData(OTSTypes.STRING, testVal));
            Assert.That(cfg.Get()!.As<string>(), Is.EqualTo(testVal));
        }

        [Test]
        public void MapConst()
        {
            string boolKey = "BoolVal";
            string stringKey = "StringVal";
            string unsignedKey = "UnsignedVal";

            bool boolVal = true;
            string stringVal = "Test String";
            ulong unsignedVal = 254734;

            var dictVal = new Dictionary<string, object>{ 
                [boolKey] = new OTSData(OTSTypes.BOOL, boolVal),  
                [stringKey] = new OTSData(OTSTypes.STRING, stringVal),  
                [unsignedKey] = new OTSData(OTSTypes.UNSIGNED, unsignedVal),  
            };

            var cfg = _mapProvider.GetConfig("Value");
            Assert.That(cfg, Is.Not.Null);

            cfg.Set(new OTSData(OTSTypes.MAP, dictVal));
            
            var retData = cfg.Get();
            Assert.That(retData, Is.Not.Null);

            var retDict = retData.As<Dictionary<string, object>>();

            Assert.Multiple(() =>
            {
                Assert.That(retDict!.TryGetValue(boolKey, out var rbool), Is.True);
                Assert.That(retDict.TryGetValue(stringKey, out var rstr), Is.True);
                Assert.That(retDict.TryGetValue(unsignedKey, out var rulong), Is.True);

                OTSData rboolVal = (OTSData)rbool!;
                OTSData rstrVal = (OTSData)rstr!;
                OTSData rulongVal = (OTSData)rulong!;

                Assert.That(rboolVal.As<bool>(), Is.EqualTo(boolVal));
                Assert.That(rstrVal.As<string>(), Is.EqualTo(stringVal));
                Assert.That(rulongVal.As<ulong>(), Is.EqualTo(unsignedVal));
            });
        }

        [Test]
        public void ListConst()
        {
            ulong unsignedVal = 200;
            long signedVal = -2500;
            double deciVal = 234.544f;

            List<object> testVal = [
                unsignedVal, signedVal, deciVal    
            ];

            var cfg = _listProvider.GetConfig("Value");
            Assert.That(cfg, Is.Not.Null);

            cfg.Set(new OTSData(OTSTypes.LIST, testVal));

            var retData = cfg.Get();
            Assert.That(retData, Is.Not.Null);

            var retArr = retData.As<List<object>>();
            Assert.Multiple(() =>
            {
                Assert.That(retArr, Is.Not.Null);
                Assert.That(retArr!, Has.Count.EqualTo(3));
                Assert.That(retArr!.ElementAt(0), Is.EqualTo(unsignedVal));
                Assert.That(retArr!.ElementAt(1), Is.EqualTo(signedVal));
                Assert.That(retArr!.ElementAt(2), Is.EqualTo(deciVal));
            });
        }
    }

}
