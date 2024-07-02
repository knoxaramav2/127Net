using OTSCommon.Plugins;
using OTSSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTSTests.StdLibTests.Math
{
    internal class SubtractionTests
    {
        private SingleSetup _setup;
        private PluginManager _pluginManager;
        private IOTSLibrary _arithLib;
        private IOTSInput _input1;
        private IOTSInput _input2;
        private IOTSOutput _output;
        private IOTSComponent _component;

        [TearDown]
        public void Cleanup() { _setup.Dispose(); }

        [SetUp]
        public void Setup()
        {
            _setup = SingleSetup.GetInstance();
            _pluginManager = new();
            try
            {
                _arithLib = _pluginManager.GetLibrary(StdLibUtils.MathLibName)!;
                _component = _arithLib.GetComponent(StdLibUtils.MathSubtraction)!;
                _input1 = _component!.GetInput("Input 1")!;
                _input2 = _component!.GetInput("Input 2")!;
                _output = _component!.GetOutput("Result")!;
            }
            catch (Exception) { Assert.Fail(); }
        }

        /*
         * Note - IO types match, Input/Output type management separate test
         */

        [Test]
        /*
        * VAL1 VAL2 SIGN EXPECTED
        *   5   10  -/+    -5
        */
        public void SubSSS()
        {
            _input1.Set(new OTSData(OTSTypes.SIGNED, 5));
            _input2.Set(new OTSData(OTSTypes.SIGNED, 10));
            _component.Update();
            var result = _output.Value!;
            Assert.That(result.As<long>(), Is.EqualTo(-5));
        }

        [Test]
        /*
        * VAL1 VAL2 SIGN EXPECTED
        *  -5   10  -/+    -15
        */
        public void SubUSS()
        {
            _input1.Set(new OTSData(OTSTypes.SIGNED, -5));
            _input2.Set(new OTSData(OTSTypes.UNSIGNED, 10));
            _component.Update();
            var result = _output.Value!;
            Assert.That(result.As<long>(), Is.EqualTo(-15));
        }

        [Test]
        /*
        * VAL1 VAL2 SIGN EXPECTED
        *   5   -10  -/+    15
        */
        public void SubSUS()
        {
            _input1.Set(new OTSData(OTSTypes.UNSIGNED, 5));
            _input2.Set(new OTSData(OTSTypes.SIGNED, -10));
            _component.Update();
            var result = _output.Value!;
            Assert.That(result.As<long>(), Is.EqualTo(15));
        }

        [Test]
        /*
        * VAL1 VAL2 SIGN EXPECTED
        *   5   10  +     -5
        */
        public void SubSSU()
        {
            _input1.Set(new OTSData(OTSTypes.UNSIGNED, 5));
            _input2.Set(new OTSData(OTSTypes.SIGNED, 10));
            _component.Update();
            var result = _output.Value!;
            Assert.That(result.As<long>(), Is.EqualTo(-5));
        }

        [Test]
        /*
        * VAL1 VAL2 SIGN EXPECTED
        *  -5  -10  -/+     5
        */
        public void SubSS()
        {
            _input1.Set(new OTSData(OTSTypes.SIGNED, -5));
            _input2.Set(new OTSData(OTSTypes.SIGNED, -10));
            _component.Update();
            var result = _output.Value!;
            Assert.That(result.As<long>(), Is.EqualTo(5));
        }

        [Test]
        /*
        * VAL1 VAL2 SIGN EXPECTED
        * MIN   1    +      0
        */
        public void SubSSSOF()
        {
            _input1.Set(new OTSData(OTSTypes.UNSIGNED, ulong.MinValue));
            _input2.Set(new OTSData(OTSTypes.UNSIGNED, 1));
            _component.Update();
            var result = _output.Value!;
            Assert.That(result.As<ulong>(), Is.EqualTo(0));
        }
    }
}
