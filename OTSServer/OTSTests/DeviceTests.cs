using OTSCommon.Database;
using OTSCommon.Security;
using OTSCommon.Models;
using Moq;
using OTSCommon.Configuration;

namespace OTSTests
{
    [TestFixture]
    public class DeviceTests
    {
        private SingleSetup _setup;
        private OTSDbService _dbService;

        private readonly string _user1 = "User1";
        private readonly string _user2 = "User2";
        private readonly string _user3 = "User3";
        private string _deviceId;

        static int nameId = 1;
        private static OTSCommon.Security.DeviceInfo CreateDeviceInfo()
        {
            var deviceMock = new Mock<OTSCommon.Security.DeviceInfo>();
            deviceMock.Setup(x => x.OsVersion).Returns("TestOs");
            deviceMock.Setup(x => x.DeviceId).Returns(Guid.NewGuid().ToString());
            deviceMock.Setup(x => x.DeviceName).Returns($"Device {nameId++}");
            return deviceMock.Object;
        }

        [TearDown]
        public void Cleanup(){ _setup.Dispose(); }

        [SetUp]
        public void Setup()
        {
            OTSConfig.LoadFromDisk();
            var realDevice = new OTSCommon.Security.DeviceInfo();
            _deviceId = realDevice.DeviceId;

            _setup = SingleSetup.GetInstance()
                .EnsureUserAccount(_user1, "ASD")
                .EnsureUserAccount(_user2, "XYZ", true)
                .EnsureUserAccount(_user3, "ZED")
                .EnsureDevice(_user1, realDevice);

            _dbService = _setup.OTSService;

            var devices = _dbService.GetDevices();
            var users = _dbService.GetUsers();

            Assert.Multiple(() =>
            {
                Assert.That(devices.Count(), Is.EqualTo(1));
                Assert.That(users.Count(), Is.EqualTo(3));
            });
        }

        [Test]
        public void DeleteAndAddDevice()
        {
            var user = _dbService.GetUser(_user1);
            Assert.That(user, Is.Not.Null);

            var deviceInfo = new OTSCommon.Security.DeviceInfo();
            _dbService.DeleteDevice(deviceInfo.DeviceId);
            Assert.That(_dbService.GetDevice(deviceInfo.DeviceId), Is.Null);
            _dbService.AddDevice(user.Id, deviceInfo);
            Assert.That(_dbService.GetDevice(deviceInfo.DeviceId), Is.Not.Null);
        }

        [Test]
        public void AddDeviceOwner()
        {
            var device = _dbService.GetDevice(_deviceId);
            Assert.That(device, Is.Not.Null);
            var ownersPre = _dbService.GetDeviceOwners(device.Id);
            Assert.Multiple(() =>
            {
                Assert.That(ownersPre, Is.Not.Null);
                Assert.That(ownersPre, Has.Count.EqualTo(1));
            });

            var owner2 = _dbService.GetUser(_user2)!;
            _dbService.AddDeviceOwner(owner2.Id, device.Id);
            
            var ownersPost = _dbService.GetDeviceOwners(device.Id);
            Assert.Multiple(() =>
            {
                Assert.That(ownersPost, Is.Not.Null);
                Assert.That(ownersPost, Has.Count.EqualTo(2));
            });
        }

        [Test]
        public void ConnectDisconnectDevices()
        {
            var info1 = CreateDeviceInfo();
            var info2 = CreateDeviceInfo();

            var user1 = _dbService.GetUser(_user1)!;
            var user2 = _dbService.GetUser(_user2)!;

            var device1Id = _dbService.AddDevice(user1.Id, info1);
            var device2Id = _dbService.AddDevice(user2.Id, info2);

            _dbService.ConnectDevices(device1Id, device2Id, user1.Id);
            var connection = _dbService.GetDeviceConnection(device2Id, device1Id);
            Assert.That(connection, Is.Not.Null);
            
            _dbService.DisconnectDevices(connection.Id);
            connection = _dbService.GetDeviceConnection(device2Id, device1Id);
            Assert.That(connection, Is.Null);
        }

        [Test]
        public void ConnectTransientDevices()
        {
            var infoC = new OTSCommon.Security.DeviceInfo();
            var infoL = CreateDeviceInfo();
            var infoR = CreateDeviceInfo();

            var userL = _dbService.GetUser(_user1)!;
            var userC = _dbService.GetUser(_user2)!;
            var userR = _dbService.GetUser(_user3)!;

            _dbService.AddDevice(userL.Id, infoL);
            _dbService.AddDevice(userR.Id, infoR);

            var deviceL = _dbService.GetDevice(infoL.DeviceId)!;
            var deviceC = _dbService.GetDevice(infoC.DeviceId)!;
            var deviceR = _dbService.GetDevice(infoR.DeviceId)!;

            //Connect C to L&R
            Assert.Multiple(() =>
            {
                Assert.That(_dbService.ConnectDevice(deviceL.Id, infoC.DeviceId, false, userC.Id, true));
                Assert.That(_dbService.ConnectDevice(deviceR.Id, infoC.DeviceId, false, userC.Id, true));
            });
            var sharedToken = Guid.NewGuid();
            
            //L request transient connection to R through C
            _dbService.AddTransientCertificate(
                deviceL.Id, deviceR.Id, sharedToken);

            var certificate = _dbService.GetTransientCertificate(deviceL.Id, deviceR.Id);
            
            Assert.That(certificate, Is.Not.Null);
        }
    }
}
