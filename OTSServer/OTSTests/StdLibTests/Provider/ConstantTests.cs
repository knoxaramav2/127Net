using Google.Protobuf.WellKnownTypes;
using Org.BouncyCastle.Asn1.Ocsp;
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
        private SingleSetupPlugins _setupPlugins;

        [TearDown]
        public void Cleanup() { _setup.Dispose(); }

        [SetUp]
        public void Setup()
        {
            _setup = SingleSetup.GetInstance()
                .EnsurePlugins()
                .EnsureLibrary(StdLibUtils.ProvidersLibName)
                .EnsureComponent(StdLibUtils.ProvidersLibName, StdLibUtils.ProvidersConstSigned)
                .EnsureComponent(StdLibUtils.ProvidersLibName, StdLibUtils.ProvidersConstUnsigned)
                .EnsureComponent(StdLibUtils.ProvidersLibName, StdLibUtils.ProvidersConstDecimal)
                .EnsureComponent(StdLibUtils.ProvidersLibName, StdLibUtils.ProvidersConstBool)
                .EnsureComponent(StdLibUtils.ProvidersLibName, StdLibUtils.ProvidersConstString)
                .EnsureComponent(StdLibUtils.ProvidersLibName, StdLibUtils.ProvidersConstList)
                .EnsureComponent(StdLibUtils.ProvidersLibName, StdLibUtils.ProvidersConstMap)
                .EndPlugins;
            _setupPlugins =  _setup.PluginSetups!;
        }

        private T? SetTestValue<T>(string componentName, T testVal)
        {
            var type = TypeConversion.TypeFromGeneric<T>();
            _setupPlugins.GetComponent(componentName, out var cfg);
            cfg!.GetConfig("Value")?.Set(new OTSData(type, testVal));
            cfg.Update();
            var res = cfg.GetOutput("Result")!.Value!.As<T>();

            return res;
        }

        [Test]
        public void SignedConst()
        {
            const long testVal = -5284;
            var res = SetTestValue<long>(StdLibUtils.ProvidersConstSigned, testVal);
            Assert.That(res, Is.EqualTo(testVal));
        }

        [Test]
        public void UnsignedConst()
        {
            const ulong testVal = 20050;
            var res = SetTestValue<ulong>(StdLibUtils.ProvidersConstUnsigned, testVal);
            Assert.That(res, Is.EqualTo(testVal));
        }

        [Test]
        public void DoubleConst()
        {
            const double testVal = 200.54f;
            var res = SetTestValue<double>(StdLibUtils.ProvidersConstDecimal, testVal);
            Assert.That(res, Is.EqualTo(testVal));
        }

        [Test]
        public void BoolConst()
        {
            const bool testVal = true;
            var res = SetTestValue<bool>(StdLibUtils.ProvidersConstBool, testVal);
            Assert.That(res, Is.EqualTo(testVal));
        }

        [Test]
        public void StringConst()
        {
            const string testVal = "ThisIs A test String!";
            var res = SetTestValue<string>(StdLibUtils.ProvidersConstString, testVal);
            Assert.That(res, Is.EqualTo(testVal));
        }

        [Test]
        public void ListConst()
        {
            List<object> testVal = [1, 2];
            var res = SetTestValue<List<object>>(StdLibUtils.ProvidersConstList, testVal);
            Assert.That(res, Is.EquivalentTo(testVal));
        }

        [Test]
        public void MapConst()
        {
            Dictionary<string, object> testVal = new()
            {
                ["Value 1"] = 2,
                ["Value 2"] = 3.6,
            };

            var res = SetTestValue<Dictionary<string, object>>(StdLibUtils.ProvidersConstMap, testVal);
            Assert.That(res, Is.EquivalentTo(testVal));
        }
    }

}
