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
    internal class ComparisonTest
    {
        private SingleSetup _setup;
        private PluginManager _pluginManager;
        private IOTSLibrary _providerLib;
        private IOTSComponent _equOp;
        private IOTSComponent _nEquOp;
        private IOTSComponent _grtrOp;
        private IOTSComponent _lessOp;
        private IOTSComponent _grtrEquOp;
        private IOTSComponent _lessEquOp;

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
                _equOp = _providerLib.GetComponent("LogicalEqual")!;
                _nEquOp = _providerLib.GetComponent("LogicalNotEqual")!;
                _lessOp = _providerLib.GetComponent("LogicalLess")!;
                _grtrOp = _providerLib.GetComponent("LogicalGreater")!;
                _lessEquOp = _providerLib.GetComponent("LogicalLessEqual")!;
                _grtrEquOp = _providerLib.GetComponent("LogicalGreaterEqual")!;

            }
            catch (Exception) { Assert.Fail(); }
        }


        #region EQUAL
        private bool TestEquOperator(long lValue, long rValue)
        {
            var input1 = _equOp.GetInput("Input 1");
            var input2 = _equOp.GetInput("Input 2");
            var result = _equOp.GetOutput("Result");

            input1?.Set(new OTSData(OTSTypes.SIGNED, lValue));
            input2?.Set(new OTSData(OTSTypes.SIGNED, rValue));

            var res = result?.Get()?.As<bool>();

            return res ?? false;
        }

        [Test]
        public void EquTest111() => Assert.That(TestEquOperator(500, 500), Is.True);
        [Test]
        public void EquTest100() => Assert.That(TestEquOperator(500, 200), Is.False);
        [Test]
        public void EquTest010() => Assert.That(TestEquOperator(200, 500), Is.False);


        #endregion


        #region NOT EQUAL

        private bool TestNotEquOperator(string lValue, string rValue)
        {
            var input1 = _nEquOp.GetInput("Input 1");
            var input2 = _nEquOp.GetInput("Input 2");
            var result = _nEquOp.GetOutput("Result");

            input1?.Set(new OTSData(OTSTypes.STRING, lValue));
            input2?.Set(new OTSData(OTSTypes.STRING, rValue));

            var res = result?.Get()?.As<bool>();

            return res ?? false;
        }

        [Test]
        public void NotEquTest110() => Assert.That(TestNotEquOperator("Hello", "Hello"), Is.False);
        [Test]
        public void NotEquTest101() => Assert.That(TestNotEquOperator("Hello", "World"), Is.True);
        [Test]
        public void NotEquTest011() => Assert.That(TestNotEquOperator("World", "Hello"), Is.True);

        #endregion


        #region GREATER

        private bool TestGrtrOperator(long lValue, long rValue)
        {
            var input1 = _grtrOp.GetInput("Input 1");
            var input2 = _grtrOp.GetInput("Input 2");
            var result = _grtrOp.GetOutput("Result");

            input1?.Set(new OTSData(OTSTypes.SIGNED, lValue));
            input2?.Set(new OTSData(OTSTypes.SIGNED, rValue));

            var res = result?.Get()?.As<bool>();

            return res ?? false;
        }

        [Test]
        public void GtrTest110() => Assert.That(TestGrtrOperator(500, 500), Is.False);
        [Test]
        public void GtrTest101() => Assert.That(TestGrtrOperator(500, 200), Is.True);
        [Test]
        public void GtrTest010() => Assert.That(TestGrtrOperator(200, 500), Is.False);

        #endregion


        #region LESS

        private bool TestLessOperator(long lValue, long rValue)
        {
            var input1 = _lessOp.GetInput("Input 1");
            var input2 = _lessOp.GetInput("Input 2");
            var result = _lessOp.GetOutput("Result");

            input1?.Set(new OTSData(OTSTypes.SIGNED, lValue));
            input2?.Set(new OTSData(OTSTypes.SIGNED, rValue));

            var res = result?.Get()?.As<bool>();

            return res ?? false;
        }

        [Test]
        public void LssTest110() => Assert.That(TestLessOperator(500, 500), Is.False);
        [Test]
        public void LssTest100() => Assert.That(TestLessOperator(500, 200), Is.False);
        [Test]
        public void LssTest011() => Assert.That(TestLessOperator(200, 500), Is.True);

        #endregion


        #region GREATER EQUAL

        private bool TestGrtrEquOperator(long lValue, long rValue)
        {
            var input1 = _grtrEquOp.GetInput("Input 1");
            var input2 = _grtrEquOp.GetInput("Input 2");
            var result = _grtrEquOp.GetOutput("Result");

            input1?.Set(new OTSData(OTSTypes.SIGNED, lValue));
            input2?.Set(new OTSData(OTSTypes.SIGNED, rValue));

            var res = result?.Get()?.As<bool>();

            return res ?? false;
        }

        [Test]
        public void GtrEquTest111() => Assert.That(TestGrtrEquOperator(500, 500), Is.True);
        [Test]
        public void GtrEquTest101() => Assert.That(TestGrtrEquOperator(500, 200), Is.True);
        [Test]
        public void GtrEquTest010() => Assert.That(TestGrtrEquOperator(200, 500), Is.False);

        #endregion


        #region LESS EQUAL

        private bool TestLessEquOperator(long lValue, long rValue)
        {
            var input1 = _lessEquOp.GetInput("Input 1");
            var input2 = _lessEquOp.GetInput("Input 2");
            var result = _lessEquOp.GetOutput("Result");

            input1?.Set(new OTSData(OTSTypes.SIGNED, lValue));
            input2?.Set(new OTSData(OTSTypes.SIGNED, rValue));

            var res = result?.Get()?.As<bool>();

            return res ?? false;
        }

        [Test]
        public void LssEquTest111() => Assert.That(TestLessEquOperator(500, 500), Is.True);
        [Test]
        public void LssEquTest100() => Assert.That(TestLessEquOperator(500, 200), Is.False);
        [Test]
        public void LssEquTest011() => Assert.That(TestLessEquOperator(200, 500), Is.True);

        #endregion
    }
}
