using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Mysqlx.Crud;
using Org.BouncyCastle.Asn1.X509.Qualified;
using OTSCommon.Configuration;
using OTSCommon.Models;
using OTSCommon.Security;
using System.Data.Common;
using System.Reflection.Metadata.Ecma335;

namespace OTSCommon.Database
{
    public class OTSDbCtx(DbContextOptions<OTSDbCtx> options, IConfiguration config) : DbContext(options)
    {
        private readonly IConfiguration _config = config;

        public virtual DbSet<UserAccount> Users { get; set; }
        public virtual DbSet<UserSettings> UserSettings { get; set; }
        public virtual DbSet<RoleAuthority> RoleAuthorities { get; set; }
        public virtual DbSet<SignIn> SignIns { get; set; }
        public virtual DbSet<Device> Devices { get; set; }
        public virtual DbSet<DeviceOwner> DeviceOwners { get; set; }
        public virtual DbSet<DeviceConnection> DeviceConnections { get; set; }
        public virtual DbSet<PeerContract> PeerContracts { get; set; }
        public virtual DbSet<TransientCertificate> TransientCertificates { get; set; }
        public virtual DbSet<OTSComponent> OTSComponents { get; set; }
        public virtual DbSet<OTSApplication> OTSApplication { get; set; }
        public virtual DbSet<OTSAppDependencies> OTSAppDependencies { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserAccount>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasOne(x => x.MaxAuthority)
                    .WithMany(x => x.MaximumAuthorities)
                    .HasForeignKey(x => x.MaxAuthorityId)
                    .OnDelete(DeleteBehavior.Restrict);
                e.HasOne(x => x.OperatingAuthority)
                    .WithMany(x => x.OperatingAuthorities)
                    .HasForeignKey(x => x.OperatingAuthorityId)
                    .OnDelete(DeleteBehavior.Restrict);
                e.HasMany(x => x.DeviceOwners)
                    .WithOne(x => x.Owner)
                    .HasForeignKey(x => x.OwnerId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<UserSettings>( e =>
            {
                e.HasKey(x => x.Id);
                e.HasOne(x => x.UserAccount)
                    .WithOne(x => x.UserSettings)
                    .HasForeignKey<UserSettings>(x => x.Id)
                    .IsRequired();
            });

            builder.Entity<RoleAuthority>(e =>
            {
                e.HasKey( x => x.Id);
                e.HasOne(x => x.Downgrade)
                    .WithMany(x => x.RoleAuthorities)
                    .HasForeignKey(x => x.DowngradeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Device>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasMany(x => x.Owners)
                    .WithOne(x => x.Device)
                    .HasForeignKey(x => x.DeviceId)
                    .OnDelete(DeleteBehavior.Restrict);
                e.HasMany(x => x.Connections1)
                    .WithOne(x => x.Device1)
                    .HasForeignKey(x => x.DeviceId1)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasMany(x => x.Connections2)
                    .WithOne(x => x.Device2)
                    .HasForeignKey(x => x.DeviceId2)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasMany(x => x.PeerContracts)
                    .WithOne(x => x.Peer)
                    .HasForeignKey(x => x.PeerId)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasMany(x => x.TransientRefererCertificates)
                    .WithOne(x => x.Referer)
                    .HasForeignKey(x => x.RefererId)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasMany(x => x.TransientClientCertificates)
                    .WithOne(x => x.Client)
                    .HasForeignKey(x => x.ClientId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<DeviceOwner>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasOne(x => x.Device)
                    .WithMany(x => x.Owners)
                    .HasForeignKey(x => x.DeviceId)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasOne(x => x.Owner)
                    .WithMany(x => x.DeviceOwners)
                    .HasForeignKey(x => x.OwnerId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<DeviceConnection>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasOne(x => x.Device1)
                    .WithMany(x => x.Connections1)
                    .HasForeignKey(x => x.DeviceId1)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasOne(x => x.Device2)
                    .WithMany(x => x.Connections2)
                    .HasForeignKey(x => x.DeviceId2)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<TransientCertificate>(e =>{ 
                e.HasKey(x => x.Id);
                e.HasOne(x => x.Referer)
                    .WithMany(x => x.TransientRefererCertificates)
                    .HasForeignKey(x => x.RefererId)
                    .OnDelete(DeleteBehavior.Restrict);
                e.HasOne(x => x.Client)
                    .WithMany(x => x.TransientClientCertificates)
                    .HasForeignKey(x => x.ClientId)
                    .OnDelete(DeleteBehavior.Restrict);

            });

            builder.Entity<PeerContract>(e =>{ 
                e.HasKey(x => x.Id);
                e.HasOne(x => x.Peer)
                    .WithMany(x => x.PeerContracts)
                    .HasForeignKey(x => x.PeerId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<OTSComponent>(e =>
            {
                e.HasKey(x => x.Id);    
            });

            builder.Entity<OTSApplication>(e =>
            {
                e.HasKey(x => x.Id);    
            });

            builder.Entity<OTSAppDependencies>(e =>
            {
                e.HasKey(x => x.Id);    
            });

            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (!builder.IsConfigured)
            {
                var connStr = _config["ConnectionStrings:OTSDb"] ??
                    throw new InvalidOperationException("Unable to retrieve DB connection string.");
                builder.UseMySQL(connStr);
            }
        }
    }

    public class OTSService
    {
        private OTSDbCtx DbCtx { get; set; }

        public OTSService(OTSDbCtx ctx)
        {
            DbCtx = ctx;

            VerifyDefaultAuthorities();
        }

        //User account methods
        public int AddUser(string username, string password, RoleAuthority maxAuthority, RoleAuthority currentAuthority)
        {
            if(DbCtx.Users.Any(x => x.NormalizedUsername.Equals(username, StringComparison.CurrentCultureIgnoreCase)
             && x.DeletedOn == null)) {
                return -1;
            }

            var salt = Irreversible.GenerateSalt();
            var hash = Irreversible.CreateSaltedHash(password, salt);


            var user = new UserAccount
            {
                Username = username,
                NormalizedUsername = username.ToUpper(),
                PassHash = hash,
                PassSalt = salt,
                MaxAuthority = maxAuthority,
                OperatingAuthority = currentAuthority,
                UserSettings = UserSettings.GetDefaultSettings()
            };

            var settings = new UserSettings
            {
                Id = user.Id,
                AutoSignIn = true,
                UserAccount = user,
            };

            user.UserSettings = settings;

            DbCtx.Users.Add(user);
            DbCtx.UserSettings.Add(settings);

            DbCtx.SaveChanges();

            return user?.Id ?? -1;
        }

        public UserAccount? GetUser(string username) =>
            DbCtx.Users.FirstOrDefault(x => x.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        public UserAccount? GetUser(int id) => DbCtx.Users.FirstOrDefault(x => x.Id == id && x.DeletedOn == null);
        public ICollection<UserAccount> GetUsers() => [.. DbCtx.Users.Where(x => x.DeletedOn == null)];


        //User settings methods
        public int AddUserSettings(int userId)
        {
            var user = DbCtx.Users.FirstOrDefault(x => x.Id == userId && x.DeletedOn == null);
            if (user == default || DbCtx.UserSettings.Any(x => x.Id == userId && x.DeletedOn == null)) { return -1; }
            
            var settings = new UserSettings
            {
                Id = userId,
                AutoSignIn = true,
                UserAccount = user
            };

            DbCtx.UserSettings.Add(settings);

            return settings?.Id ?? -1;
        }


        //Role authority methods
        private void VerifyDefaultAuthorities()
        {
            var authMeta = new List<(string, int)>{ ("Guest", 100), ("Owner", 50), ("Admin", 0) };
            List<RoleAuthority?> authTmp = [];

            for(var i = 0; i < authMeta.Count; ++i)
            {
                var authDat = authMeta[i];

                var auth = GetRoleAuthority(authDat.Item1);
                if(auth == null)
                {
                    var prev = i > 0 ? authTmp[i-1] : null;

                    auth = new RoleAuthority
                    {
                        RoleName = authDat.Item1,
                        AuthLevel = authDat.Item2,
                        Downgrade = prev,
                        DowngradeId = prev?.Id,
                    };

                    auth = DbCtx.RoleAuthorities.Add(auth).Entity;
                } 

                auth.Id = i+1;
                authTmp.Add(auth);
            }

            DbCtx.SaveChanges();
        }

        public bool AddRoleAuthority(string roleName, int authLevel, int? downgradeId=null)
        {
            if(DbCtx.RoleAuthorities.Any(x => x.RoleName.Equals(roleName, StringComparison.OrdinalIgnoreCase) && x.DeletedOn == null)) { return false; }
            
            var downgrade = downgradeId != null?
                GetRoleAuthority(downgradeId.Value) : null;

            var role = new RoleAuthority
            {
                RoleName = roleName,
                AuthLevel = authLevel,
                DowngradeId = downgradeId,
                Downgrade = downgrade
            };

            DbCtx.RoleAuthorities.Add(role);

            return DbCtx.SaveChanges() == 1;
        }

        public Tuple<RoleAuthority, RoleAuthority>? GetDefaultAuthorityPair()
        {
            var maxAuth = DbCtx.RoleAuthorities.First(x => x.RoleName.Equals("Basic"));
            var opAuth = DbCtx.RoleAuthorities.First(x => x.RoleName.Equals("Guest"));

            if(maxAuth == null || opAuth == null) { return null; }

            return new Tuple<RoleAuthority, RoleAuthority>(maxAuth, opAuth);
        }
    
        public RoleAuthority? GetRoleAuthority(int roleId)
        {
            return DbCtx.RoleAuthorities.FirstOrDefault(x => x.Id == roleId && x.DeletedOn == null);
        }

        public RoleAuthority? GetRoleAuthority(string roleName)
        {
            return DbCtx.RoleAuthorities.FirstOrDefault(x => x.RoleName.Equals(roleName, StringComparison.OrdinalIgnoreCase)
                 && x.DeletedOn == null);
        }

        public List<RoleAuthority> GetRoleAuthorities()
        {
            return [.. DbCtx.RoleAuthorities.Where(x => x.DeletedOn == null)];
        }
    
        public bool UpdateUserRole(int roleId, int userId)
        {
            var user = DbCtx.Users.FirstOrDefault(x => x.Id == userId && x.DeletedOn == null);
            var role = DbCtx.RoleAuthorities.FirstOrDefault(x => x.Id == roleId && x.DeletedOn == null);
            if (user == null || role == null || role.AuthLevel < user.MaxAuthority.AuthLevel) 
                { return false; }

            user.OperatingAuthority = role;
            DbCtx.Users.Update(user);

            return true;
        }

        //Sign in methods
        public int AddSignIn(int userId)
        {
            var user = DbCtx.Users.FirstOrDefault(x => x.Id == userId);
            if (user == null) { return -1; }

            var log = new SignIn
            {
                UserAccount = user,
                UserId = userId,
                SiginInDate = DateTime.UtcNow,
            };

            DbCtx.SignIns.Add(log);
            DbCtx.SaveChanges();

            return log?.Id ?? -1;
        }
    
        public SignIn? GetLastSignIn()
        {
            return DbCtx.SignIns.OrderByDescending(x => x.SiginInDate)
                .FirstOrDefault();
        }
        
        //Device methods
        public int AddDevice(int ownerId, IDeviceInfo deviceInfo)
        {
            var owner = DbCtx.Users.FirstOrDefault(x => x.Id == ownerId && x.DeletedOn == null);
            if (owner == null || DbCtx.Devices.Any(x => x.HwId == deviceInfo.DeviceId && x.DeletedOn == null)) 
                { return -1; }    

            var device = new Device
            {
                HwId = deviceInfo.DeviceId,
                DisplayName = deviceInfo.DeviceName,
                Os = deviceInfo.OsVersion
            };

            var deviceOwner = new DeviceOwner
            {
                Device = device,
                DeviceId = device.Id,
                Owner = owner,
                OwnerId = ownerId,
            };

            DbCtx.Devices.Add(device);
            DbCtx.DeviceOwners.Add(deviceOwner);
            DbCtx.SaveChanges();

            return device?.Id ?? -1;
        }

        public bool DeleteDevice(string deviceId)
        {
            var device = DbCtx.Devices.FirstOrDefault(x => x.HwId == deviceId && x.DeletedOn == null);
            if(device == null) { return false; }

            device.DeletedOn = DateTime.UtcNow;
            DbCtx.Devices.Update(device);
            DbCtx.SaveChanges();

            return true;
        }

        public Device? GetDevice(string hwId) => 
            DbCtx.Devices.FirstOrDefault(x => x.HwId == hwId && x.DeletedOn == null);
        public Device? GetDevice(int id) =>
            DbCtx.Devices.FirstOrDefault(x => x.Id == id && x.DeletedOn == null);

        public ICollection<Device> GetDevices() => [.. DbCtx.Devices.Where(x => x.DeletedOn == null)];

        public int AddDeviceOwner(int ownerId, int deviceId)
        {
            var device = GetDevice(deviceId);
            var owner = GetUser(ownerId);

            if(device == null || owner == null ||
                DbCtx.DeviceOwners.Any(x => x.DeviceId == deviceId && x.OwnerId == ownerId)
                ) { return -1; }

            var deviceOwner = new DeviceOwner
            {
                Device = device,
                DeviceId = device.Id,
                Owner = owner,
                OwnerId = ownerId,
            };

            var t = DbCtx.DeviceOwners.Add(deviceOwner);
            DbCtx.SaveChanges();

            var all = DbCtx.DeviceOwners.ToList();

            return deviceOwner?.DeviceId ?? -1;
        }
    
        public ICollection<DeviceOwner> GetDeviceOwners(int deviceId ) =>
            [.. DbCtx.DeviceOwners.Where(x => x.DeviceId == deviceId && x.DeletedOn == null)];
    
        public bool ConnectDevices(int device1Id, int device2Id, int requesterId)
        {
            var assigner = DbCtx.Users.FirstOrDefault(x => x.Id == requesterId && x.DeletedOn == null);
            var device1 = DbCtx.Devices.FirstOrDefault(x => x.Id == device1Id && x.DeletedOn == null);
            var device2 = DbCtx.Devices.FirstOrDefault(x => x.Id == device2Id && x.DeletedOn == null);

            if(GetDeviceConnection(device1Id, device2Id) != null || assigner == null
                || device1 == null || device2 == null) { return false; }

            var connection = new DeviceConnection
            {
                AssignedBy = assigner,
                AssignedById = requesterId,
                Device1 = device1,
                DeviceId1 = device1Id,
                Device2 = device2,
                DeviceId2 = device2Id,
            };

            DbCtx.DeviceConnections.Add(connection);
            DbCtx.SaveChanges();

            return true;
        }

        public bool ConnectDevice(int peerId, string deviceId, bool isRequester, int userId, bool allowWrite)
        {
            var currDevice = GetDevice(deviceId)!;
            var assignerId = isRequester ? currDevice.Id : peerId;
            var localUser = GetUser(userId)!;

            if (!ConnectDevices(peerId, currDevice.Id, assignerId) || 
                (allowWrite && localUser.OperatingAuthority.AuthLevel > OTSConfig.AuthWriteLevel)) { return false; }
            if(!AddPeerContract(peerId, Guid.NewGuid(), userId, allowWrite)) { return false; }

            return true;
        }

        public bool DisconnectDevices(int connectionId)
        {
            var connection = DbCtx.DeviceConnections.FirstOrDefault(x => x.Id == connectionId);
            if(connection == null) return false;

            connection.DeletedOn = DateTime.UtcNow;
            DbCtx.DeviceConnections.Update(connection);

            DbCtx.SaveChanges();

            return true;
        }

        public DeviceConnection? GetDeviceConnection(int deviceId1, int deviceId2) =>
            DbCtx.DeviceConnections.FirstOrDefault(x => 
                (x.DeviceId1 == deviceId1 && x.DeviceId2 == deviceId2) || 
                (x.DeviceId1 == deviceId2 && x.DeviceId2 == deviceId1) && x.DeletedOn == null);
       
        public bool AddPeerContract(int peerId, Guid connectionToken, 
            int localUserId, bool allowWrite, 
            DateTime? expiration = null)
        {
            var localUser = DbCtx.Users.FirstOrDefault(x => x.Id == localUserId && x.DeletedOn == null);
            var peerDevice = DbCtx.Devices.FirstOrDefault(x => x.Id == peerId && x.DeletedOn == null);
            if(peerDevice == null || localUser == null) return false;

            if (allowWrite && localUser.OperatingAuthority.AuthLevel > OTSConfig.AuthWriteLevel) { return false; }

            var contract = new PeerContract
            {
                Peer = peerDevice,
                PeerId = peerId,
                Token = connectionToken,
                Issued = DateTime.UtcNow,
                AllowWrite = allowWrite,
                Expiration = expiration ?? DateTime.UtcNow.AddDays(30),
                Authorizer = localUser,
                AuthorizerId = localUserId,
            };
            
            DbCtx.PeerContracts.Add(contract);
            DbCtx.SaveChanges();

            return true;
        } 

        public PeerContract? GetPeerContract(int contractId) =>
            DbCtx.PeerContracts.FirstOrDefault(x => x.Id == contractId && x.DeletedOn == null && x.Expiration > DateTime.UtcNow);

        public PeerContract? GetPeerContractByDevice(int deviceId) =>
            DbCtx.PeerContracts.FirstOrDefault(x => x.PeerId == deviceId && x.DeletedOn == null && x.Expiration > DateTime.UtcNow);

        public bool AddTransientCertificate(int refererDeviceId, int clientDeviceId,
            Guid SharedToken, DateTime? expiration=null)
        {
            var referer = GetDevice(refererDeviceId);
            var client = GetDevice(clientDeviceId);

            var contracts = DbCtx.PeerContracts.ToList();

            var clientContract = GetPeerContractByDevice(clientDeviceId);
            var refererContract = GetPeerContractByDevice(refererDeviceId);

            if (referer == null || client == null || 
                clientContract == null || refererContract == null)
                { return false; }

            var certificate = new TransientCertificate
            {
                Client = client,
                ClientId = clientDeviceId,
                Referer = referer,
                RefererId = refererDeviceId,
                Expiration = expiration ?? DateTime.UtcNow.AddDays(30),
                Issued = DateTime.UtcNow,
                SharedPeerToken = SharedToken,
            };

            DbCtx.TransientCertificates.Add(certificate);
            DbCtx.SaveChanges();

            return true;
        }
    
        public TransientCertificate? GetTransientCertificate(int refererDeviceId, int clientDeviceId) =>
            DbCtx.TransientCertificates.FirstOrDefault(x => x.RefererId == refererDeviceId && x.ClientId == clientDeviceId);
    
        
        //Component methods
        

        //Application methods
    }
}
