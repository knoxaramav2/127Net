using Moq;
using OTSCommon.Database;
using OTSCommon.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OTSTests
{
    internal class ComponentTests
    {
        private SingleSetup _setup;
        private OTSDbCtx _ctx;
        private OTSService _service;

        private string _localUser = "LocalUser";
        private string _remoteUser = "RemoteUser";

        [TearDown]
        public void Cleanup() { _setup.Dispose(); }

        private IDeviceInfo CreateDeviceInfo()
        {
            var rng = new Random();
            var di = new Mock<IDeviceInfo>();
            di.Setup(x => x.DeviceId).Returns(Guid.NewGuid().ToString());
            di.Setup(x => x.OsVersion).Returns("OTS OS");
            di.Setup(x => x.DeviceName).Returns($"Device {rng.Next(100, 999)}");
            
            return di.Object;
        }

        [SetUp]
        public void Setup()
        {
            _setup = SingleSetup.GetInstance()
                .EnsureUserAccount(_localUser, "ASD123", true)
                .EnsureUserAccount(_remoteUser, "ASD123", false)
                .EnsureDevice(_localUser, CreateDeviceInfo())
                .EnsureDevice(_remoteUser, CreateDeviceInfo())
                ;

        }

        [Test]
        public void Test()
        {
            Assert.True(true);
        }
    }
}
