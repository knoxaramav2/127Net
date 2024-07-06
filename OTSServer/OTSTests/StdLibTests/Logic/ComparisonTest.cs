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
        private SingleSetupPlugins _plugins;

        [TearDown]
        public void Cleanup() { _setup.Dispose(); }

        [SetUp]
        public void Setup()
        {
            _setup = SingleSetup.GetInstance()
                .EnsurePlugins()
                .EnsureLibrary(StdLibUtils.LogicLibName)
                .EnsureComponent(StdLibUtils.LogicLibName, StdLibUtils.LogicalEqual)
                .EnsureComponent(StdLibUtils.LogicLibName, StdLibUtils.LogicalNotEqual)
                .EnsureComponent(StdLibUtils.LogicLibName, StdLibUtils.LogicalLess)
                .EnsureComponent(StdLibUtils.LogicLibName, StdLibUtils.LogicalGreater)
                .EnsureComponent(StdLibUtils.LogicLibName, StdLibUtils.LogicalLessEqual)
                .EnsureComponent(StdLibUtils.LogicLibName, StdLibUtils.LogicalGreaterEqual)
                .EndPlugins;
             _plugins = _setup.PluginSetups!;
        }

        private bool testOperator<T, U>(string componentName, T lVal, U rVal)
        {
            _plugins.GetComponent(componentName, out var op);
            var type = TypeConversion.TypeFromGeneric<T>();

            var in1 = op!.GetInput("Input 1")!;
            var in2 = op!.GetInput("Input 2")!;
            var result = op!.GetOutput("Result");

            in1.Set(new OTSData(type, lVal));
            in2.Set(new OTSData(type, rVal));
            op.Update();
            var res = result?.Value?.As<bool>();

            return res ?? false;
        }


        #region EQUAL

        [Test]
        public void EquTest111() => Assert.That(testOperator(StdLibUtils.LogicalEqual, 500, 500), Is.True);
        [Test]
        public void EquTest100() => Assert.That(testOperator(StdLibUtils.LogicalEqual, 500, 200), Is.False);
        [Test]
        public void EquTest010() => Assert.That(testOperator(StdLibUtils.LogicalEqual, 200, 500), Is.False);


        #endregion


        #region NOT EQUAL

        [Test]
        public void NotEquTest110() => Assert.That(testOperator(StdLibUtils.LogicalNotEqual, "Hello", "Hello"), Is.False);
        [Test]
        public void NotEquTest101() => Assert.That(testOperator(StdLibUtils.LogicalNotEqual, "Hello", "World"), Is.True);
        [Test]
        public void NotEquTest011() => Assert.That(testOperator(StdLibUtils.LogicalNotEqual, "World", "Hello"), Is.True);

        #endregion


        #region GREATER

        [Test]
        public void GtrTest110() => Assert.That(testOperator(StdLibUtils.LogicalGreater, 500, 500), Is.False);
        [Test]
        public void GtrTest101() => Assert.That(testOperator(StdLibUtils.LogicalGreater, 500, 200), Is.True);
        [Test]
        public void GtrTest010() => Assert.That(testOperator(StdLibUtils.LogicalGreater, 200, 500), Is.False);

        #endregion


        #region LESS

        [Test]
        public void LssTest110() => Assert.That(testOperator(StdLibUtils.LogicalLess, 500, 500), Is.False);
        [Test]
        public void LssTest100() => Assert.That(testOperator(StdLibUtils.LogicalLess, 500, 200), Is.False);
        [Test]
        public void LssTest011() => Assert.That(testOperator(StdLibUtils.LogicalLess, 200, 500), Is.True);

        #endregion


        #region GREATER EQUAL

        [Test]
        public void GtrEquTest111() => Assert.That(testOperator(StdLibUtils.LogicalGreaterEqual, 500, 500), Is.True);
        [Test]
        public void GtrEquTest101() => Assert.That(testOperator(StdLibUtils.LogicalGreaterEqual, 500, 200), Is.True);
        [Test]
        public void GtrEquTest010() => Assert.That(testOperator(StdLibUtils.LogicalGreaterEqual, 200, 500), Is.False);

        #endregion


        #region LESS EQUAL

        [Test]
        public void LssEquTest111() => Assert.That(testOperator(StdLibUtils.LogicalLessEqual, 500, 500), Is.True);
        [Test]
        public void LssEquTest100() => Assert.That(testOperator(StdLibUtils.LogicalLessEqual, 500, 200), Is.False);
        [Test]
        public void LssEquTest011() => Assert.That(testOperator(StdLibUtils.LogicalLessEqual, 200, 500), Is.True);

        #endregion
    }
}
