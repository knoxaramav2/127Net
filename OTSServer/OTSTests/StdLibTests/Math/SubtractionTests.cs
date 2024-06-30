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
        private IOTSConfigField _configField;
        private IOTSInput _input1;
        private IOTSInput _input2;
        private IOTSOutput _output;

        [TearDown]
        public void Cleanup() { _setup.Dispose(); }

        [SetUp]
        public void Setup()
        {
            _setup = SingleSetup.GetInstance();
            _pluginManager = new();
            try
            {
                _arithLib = _pluginManager.GetLibrary("OTSMath")!;
                var component = _arithLib.GetComponent("Subtraction");
                _configField = component!.GetConfig("SignToggleConfig")!;
                _input1 = component!.GetInput("Input 1")!;
                _input2 = component!.GetInput("Input 2")!;
                _output = component!.GetOutput("Result")!;
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
            _configField.Set(new OTSData(OTSTypes.BOOL, true));
            _input1.Set(new OTSData(OTSTypes.SIGNED, 5));
            _input2.Set(new OTSData(OTSTypes.SIGNED, 10));

            var result = _output.Get()!;
            Assert.That(result.As<long>(), Is.EqualTo(-5));
        }

        [Test]
        /*
        * VAL1 VAL2 SIGN EXPECTED
        *  -5   10  -/+    -15
        */
        public void SubUSS()
        {
            _configField.Set(new OTSData(OTSTypes.BOOL, true));
            _input1.Set(new OTSData(OTSTypes.SIGNED, -5));
            _input2.Set(new OTSData(OTSTypes.UNSIGNED, 10));

            var result = _output.Get()!;
            Assert.That(result.As<long>(), Is.EqualTo(-15));
        }

        [Test]
        /*
        * VAL1 VAL2 SIGN EXPECTED
        *   5   -10  -/+    15
        */
        public void SubSUS()
        {
            _configField.Set(new OTSData(OTSTypes.BOOL, true));
            _input1.Set(new OTSData(OTSTypes.UNSIGNED, 5));
            _input2.Set(new OTSData(OTSTypes.SIGNED, -10));

            var result = _output.Get()!;
            Assert.That(result.As<long>(), Is.EqualTo(15));
        }

        [Test]
        /*
        * VAL1 VAL2 SIGN EXPECTED
        *   5   -10  +      0
        */
        public void SubSSU()
        {
            _configField.Set(new OTSData(OTSTypes.BOOL, false));
            _input1.Set(new OTSData(OTSTypes.UNSIGNED, 5));
            _input2.Set(new OTSData(OTSTypes.SIGNED, 15));

            var result = _output.Get()!;
            Assert.That(result.As<long>(), Is.EqualTo(0));
        }

        [Test]
        /*
        * VAL1 VAL2 SIGN EXPECTED
        *  -5  -10  -/+     5
        */
        public void SubSS()
        {
            _configField.Set(new OTSData(OTSTypes.BOOL, true));
            _input1.Set(new OTSData(OTSTypes.SIGNED, -5));
            _input2.Set(new OTSData(OTSTypes.SIGNED, -10));

            var result = _output.Get()!;
            Assert.That(result.As<long>(), Is.EqualTo(5));
        }

        [Test]
        /*
        * VAL1 VAL2 SIGN EXPECTED
        * MIN   1    +      0
        */
        public void SubSSSOF()
        {
            _configField.Set(new OTSData(OTSTypes.BOOL, false));
            _input1.Set(new OTSData(OTSTypes.UNSIGNED, ulong.MinValue));
            _input2.Set(new OTSData(OTSTypes.UNSIGNED, 1));

            var result = _output.Get()!;
            Assert.That(result.As<ulong>(), Is.EqualTo(0));
        }
    }
}
