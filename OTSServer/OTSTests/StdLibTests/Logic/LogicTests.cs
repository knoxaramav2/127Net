using Moq;
using OTSCommon.Plugins;
using OTSSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTSTests.StdLibTests.Logic
{
    internal class LogicTests
    {
        private SingleSetup _setup;
        private PluginManager _pluginManager;
        private IOTSLibrary _providerLib;
        private IOTSComponent _andOp;
        private IOTSComponent _orOp;
        private IOTSComponent _xorOp;

        [TearDown]
        public void Cleanup() { _setup.Dispose(); }

        [SetUp]
        public void Setup()
        {
            _setup = SingleSetup.GetInstance();
            _pluginManager = new();
            try
            {
                _providerLib = _pluginManager.GetLibrary("OTSLogic")!;
                _andOp = _providerLib.GetComponent("LogicalAnd")!;
                _orOp = _providerLib.GetComponent("LogicalOr")!;
                _xorOp = _providerLib.GetComponent("LogicalXOR")!;
            }
            catch (Exception) { Assert.Fail(); }
        }


        private bool TestAndAdapter(bool lValue, bool rValue)
        {
            var input1 = _andOp.GetInput("Input 1");
            var input2 = _andOp.GetInput("Input 2");
            var result = _andOp.GetOutput("Result");

            input1?.Set(new OTSData(OTSTypes.BOOL, lValue));
            input2?.Set(new OTSData(OTSTypes.BOOL, rValue));
            _andOp.Update();
            var res = result?.Value?.As<bool>();

            return res ?? false;
        }

        [Test]
        public void AndTest111() => Assert.That(TestAndAdapter(true, true), Is.True);
        [Test]
        public void AndTest100() => Assert.That(TestAndAdapter(true, false), Is.False);
        [Test]
        public void AndTest010() => Assert.That(TestAndAdapter(false, true), Is.False);
        [Test]
        public void AndTest000() => Assert.That(TestAndAdapter(false, false), Is.False);


        private bool TestOrAdapter(bool lValue, bool rValue)
        {
            var input1 = _orOp.GetInput("Input 1");
            var input2 = _orOp.GetInput("Input 2");
            var result = _orOp.GetOutput("Result");

            input1?.Set(new OTSData(OTSTypes.BOOL, lValue));
            input2?.Set(new OTSData(OTSTypes.BOOL, rValue));
            _orOp.Update();
            var res = result?.Value?.As<bool>();

            return res ?? false;
        }

        [Test]
        public void OrTest111() => Assert.That(TestOrAdapter(true, true), Is.True);
        [Test]      
        public void OrTest101() => Assert.That(TestOrAdapter(true, false), Is.True);
        [Test]      
        public void OrTest011() => Assert.That(TestOrAdapter(false, true), Is.True);
        [Test]      
        public void OrTest000() => Assert.That(TestOrAdapter(false, false), Is.False);


        private bool TestXorAdapter(bool lValue, bool rValue)
        {
            var input1 = _xorOp.GetInput("Input 1");
            var input2 = _xorOp.GetInput("Input 2");
            var result = _xorOp.GetOutput("Result");

            input1?.Set(new OTSData(OTSTypes.BOOL, lValue));
            input2?.Set(new OTSData(OTSTypes.BOOL, rValue));
            _xorOp.Update();
            var res = result?.Value?.As<bool>();

            return res ?? false;
        }
        [Test]
        public void XorTest110() => Assert.That(TestXorAdapter(true, true), Is.False);
        [Test]      
        public void XorTest101() => Assert.That(TestXorAdapter(true, false), Is.True);
        [Test]      
        public void XorTest011() => Assert.That(TestXorAdapter(false, true), Is.True);
        [Test]      
        public void XorTest000() => Assert.That(TestXorAdapter(false, false), Is.False);
    }
}
