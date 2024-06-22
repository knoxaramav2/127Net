using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OTSCommon.Database;
using OTSCommon.Models;
using OTSCommon.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTSTests
{
    public class SingleSetup: IDisposable
    {
        private static SingleSetup? Instance { get; set; }

        public OTSDbCtx OTSDbCtx { get; private set; }
        public OTSService OTSService { get; private set; }

        private SingleSetup() 
        { 
            var config = new ConfigurationBuilder()
                .AddUserSecrets<UserTests>()
                .Build();    
            var dbOptions = new DbContextOptionsBuilder<OTSDbCtx>()
                .UseInMemoryDatabase(databaseName: "OTS_IM")
                .Options;
            OTSDbCtx = new OTSDbCtx(dbOptions, config);
            OTSDbCtx.Database.EnsureDeleted();
            OTSService = new OTSService(OTSDbCtx);
        }

        public SingleSetup EnsureUserAccount(string username, string password, bool admin=false)
        {
            var maxAuthStr = admin ? "Admin" : "Owner";
            var maxAuth = OTSService.GetRoleAuthority(maxAuthStr)!;
            var opAuth = maxAuth.Downgrade ?? maxAuth;

            OTSService.AddUser(username, password, maxAuth, opAuth);

            return this;
        }

        public SingleSetup EnsureDevice(string username, IDeviceInfo info)
        {
            var user = OTSService.GetUser(username)!;

            OTSService.AddDevice(user.Id, info);

            return this;
        }

        public static SingleSetup GetInstance()
        {
            Instance ??= new SingleSetup();
            return Instance;
        }
    
        public void Dispose()
        {
            OTSDbCtx.Dispose();
            Instance = null;
        }    
    }
}
