using NuGet.Frameworks;
using OTSCommon.Plugins;
using OTSSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace OTSTests.StdLibTests.Math
{
    internal class AdditionTests
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
                var component = _arithLib.GetComponent("Addition");
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
        *   5   10  -/+    15
        */
        public void AddSSS()
        {
            _configField.Set(new OTSData(OTSTypes.BOOL, true));
            _input1.Set(new OTSData(OTSTypes.UNSIGNED, 5));
            _input2.Set(new OTSData(OTSTypes.UNSIGNED, 10));

            var result = _output.Get()!;
            Assert.That(result.As<long>(), Is.EqualTo(15));
        }

        [Test]
        /*
        * VAL1 VAL2 SIGN EXPECTED
        *  -5   10  -/+    5
        */
        public void AddUSS()
        {
            _configField.Set(new OTSData(OTSTypes.BOOL, true));
            _input1.Set(new OTSData(OTSTypes.SIGNED, -5));
            _input2.Set(new OTSData(OTSTypes.UNSIGNED, 10));

            var result = _output.Get()!;
            Assert.That(result.As<long>(), Is.EqualTo(5));
        }

        [Test]
        /*
        * VAL1 VAL2 SIGN EXPECTED
        *   5   -10  -/+    -5
        */
        public void AddSUS()
        {
            _configField.Set(new OTSData(OTSTypes.BOOL, true));
            _input1.Set(new OTSData(OTSTypes.UNSIGNED, 5));
            _input2.Set(new OTSData(OTSTypes.SIGNED, -10));

            var result = _output.Get()!;
            Assert.That(result.As<long>(), Is.EqualTo(-5));
        }

        [Test]
        /*
        * VAL1 VAL2 SIGN EXPECTED
        *   5   -10  +     0
        */
        public void AddSSU()
        {
            _configField.Set(new OTSData(OTSTypes.BOOL, false));
            _input1.Set(new OTSData(OTSTypes.UNSIGNED, 5));
            _input2.Set(new OTSData(OTSTypes.SIGNED, -10));

            var result = _output.Get()!;
            Assert.That(result.As<long>(), Is.EqualTo(0));
        }

        [Test]
        /*
        * VAL1 VAL2 SIGN EXPECTED
        *  -5  -10  -/+    -15
        */
        public void AddSS()
        {
            _configField.Set(new OTSData(OTSTypes.BOOL, true));
            _input1.Set(new OTSData(OTSTypes.SIGNED, -5));
            _input2.Set(new OTSData(OTSTypes.SIGNED, -10));

            var result = _output.Get()!;
            Assert.That(result.As<long>(), Is.EqualTo(-15));
        }

        [Test]
        /*
        * VAL1 VAL2 SIGN EXPECTED
        * MAX   1    +      0
        */
        public void AddSSUOF()
        {
            _configField.Set(new OTSData(OTSTypes.BOOL, false));
            _input1.Set(new OTSData(OTSTypes.UNSIGNED, ulong.MaxValue));
            _input2.Set(new OTSData(OTSTypes.UNSIGNED, 1));

            var result = _output.Get()!;
            Assert.That(result.As<long>(), Is.EqualTo(0));
        }
    }
}
