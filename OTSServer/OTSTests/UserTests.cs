using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using OTSCommon.Database;
using OTSCommon.Models;


namespace OTSTests
{
    [TestFixture]
    public class UserTests
    {
        private SingleSetup _setup;
        private OTSDbCtx _ctx;
        private OTSService _dbService;

        private string _ownerUser = "DevUser";
        private string _adminUser = "AdminUser";

        [TearDown]
        public void Cleanup()
        {
            _setup.Dispose();
        }

        [SetUp]
        public void Setup()
        {
            _setup = SingleSetup.GetInstance()
                .EnsureUserAccount(_ownerUser, "AbC123!")
                .EnsureUserAccount(_adminUser, "1337");
            _ctx = _setup.OTSDbCtx;
            _dbService = _setup.OTSService;

            var ownerAccount = _dbService.GetRoleAuthority("Owner");
            Assert.That(ownerAccount, Is.Not.Null);
            _dbService.AddUser("DevUser", "AbC123!", ownerAccount, ownerAccount);
        }

        [Test]
        public void HasDefaultAuthorities()
        {
            var authorities = _dbService.GetRoleAuthorities();
            Assert.That(authorities, Is.Not.Null);
            Assert.That(authorities, Has.Count.EqualTo(3));
        }

        [Test]
        public void AddUser()
        {
            var username = "TestUser";
            var password = "AbCx158!@?";

            var user = _dbService.GetUser(username);
            Assert.That(user, Is.Null);

            var ownerAccount = _dbService.GetRoleAuthority("Owner");
            Assert.That(ownerAccount, Is.Not.Null);
            Assert.That(ownerAccount?.Downgrade, Is.Not.Null);

            _dbService.AddUser(username, password, ownerAccount, ownerAccount);
            Assert.That(_dbService.GetUser(username), Is.Not.Null);
        }
    
        [Test]
        public void SignIn()
        {
            var user = _dbService.GetUser(_ownerUser);
            Assert.That(user, Is.Not.Null);
            _dbService.AddSignIn(user.Id);
            var log = _dbService.GetLastSignIn();
            Assert.That(log, Is.Not.Null);
            Assert.That(log.SiginInDate, Is.EqualTo(DateTime.UtcNow).Within(3).Seconds);
        }

    }
}