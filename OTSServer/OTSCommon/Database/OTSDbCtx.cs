using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OTSCommon.Configuration;
using OTSCommon.Models;
using OTSCommon.Security;
using System.Reflection.Metadata.Ecma335;

namespace OTSCommon.Database
{
    public class OTSDbCtx(DbContextOptions<OTSDbCtx> options, IConfiguration config) : DbContext(options)
    {
        private readonly IConfiguration _config = config;

        //User
        public virtual DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<IdentityUserClaim<int>> UserIdentityClaim { get; set; }

        //Role Authority
        public virtual DbSet<RoleAuthority> RoleAuthorities { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }

        //Device
        public virtual DbSet<NetworkedDevice> NetworkedDevices { get; set; }
        public virtual DbSet<NetworkedDeviceOwner> NetworkedDeviceOwners { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Authorities
            builder.Entity<UserRole>(e =>
            {
                e.HasOne(x => x.RoleAuthority)
                    .WithMany(x => x.UserRoles)
                    .HasForeignKey(x => x.RoleAuthorityId);
                e.HasOne(x => x.UserAccount)
                    .WithMany(x => x.Roles)
                    .HasForeignKey(x => x.UserAccountId);
            });

            builder.Entity<RoleAuthority>(e =>
            {
                
            });

            //Users
            builder.Entity<UserAccount>(e =>
            {
                
            });

            //Devices
            builder.Entity<NetworkedDevice>(e =>
            {
                e.HasKey(e => e.Id);
            });

            builder.Entity<NetworkedDeviceOwner>(e =>
            {
                e.HasKey(e => e.Id);
                e.HasOne(x => x.Owner)
                    .WithMany(x => x.OwnedDevices)
                    .HasForeignKey(e => e.DeviceId)
                    .OnDelete(DeleteBehavior.Restrict);
                e.HasOne(x => x.Device)
                    .WithMany(x => x.Owners)
                    .HasForeignKey(e => e.DeviceId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            //Seeds data

            builder.Entity<RoleAuthority>().HasData(
                new RoleAuthority{ Id=1, AuthLevel = 0, Name = "Admin", IsDefault = false },
                new RoleAuthority{ Id=2, AuthLevel = 50, Name = "User", IsDefault = true },
                new RoleAuthority{ Id=3, AuthLevel = 100, Name = "Guest", IsDefault = true }
                );

            var adminAccount = new UserAccount
            {
                Id = 1,
                UserName = "Root",
                NormalizedUserName = "ROOT",
                ConcurrencyStamp = Guid.NewGuid().ToString("D"),
                Email = "",
                NormalizedEmail = "",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D"),
            };

            var hasher = new PasswordHasher<UserAccount>();
            adminAccount.PasswordHash = hasher.HashPassword(adminAccount, "admin");
            builder.Entity<UserAccount>().HasData(adminAccount);

            builder.Entity<UserRole>().HasData(
                new UserRole{ Id=1, RoleAuthorityId = 1, UserAccountId = 1, ActiveRole = true }
                );

            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (!builder.IsConfigured)
            {
                var connStr = _config["ConnectionStrings:OTSServerDb"] ??
                    throw new InvalidOperationException("Unable to retrieve DB connection string.");
                builder.UseMySQL(connStr);
            }
        }
    }

    public class OTSDbService
    {
        private OTSDbCtx DbCtx { get; set; }

        public OTSDbService(OTSDbCtx ctx)
        {
            DbCtx = ctx;
        }

        //User
        public IEnumerable<UserAccount> GetUsers()
            => DbCtx.UserAccounts;

        public UserAccount? GetUser(string username)
            => DbCtx.UserAccounts.FirstOrDefault(x => username.Equals(x.UserName, StringComparison.OrdinalIgnoreCase));

        public UserAccount? GetUser(int id)
            => DbCtx.UserAccounts.FirstOrDefault(x => x.Id == id);

        //public int CreateUser(string username, string password)
        //{
        //    var user = new UserAccount
        //    {
        //        UserRoleId = roleId,
        //    };

        //    DbCtx.Add(user);
        //    DbCtx.SaveChanges();
        //    return user.Id;
        //}

        public UserAccount? GetUserById(int userId) =>
            DbCtx.UserAccounts.FirstOrDefault(x => x.Id == userId);

        public UserAccount? GetUserByAuthId(int authId) =>
            DbCtx.UserAccounts.FirstOrDefault(x => x.Id == authId);

        //Role Authority
        public RoleAuthority? GetRoleAuthority(int roleAuthId) =>
            DbCtx.RoleAuthorities.FirstOrDefault(x => x.Id == roleAuthId);
    
        public RoleAuthority? GetRoleAuthority(string authName) =>
            DbCtx.RoleAuthorities.FirstOrDefault(x => x.Name.Equals(authName, StringComparison.OrdinalIgnoreCase));

        public RoleAuthority? GetUserRole(int userId)
        {
            var userRole = DbCtx.UserRoles.FirstOrDefault(x => x.UserAccountId == userId && x.ActiveRole);
            if(userRole == null) { return null; }
            return DbCtx.RoleAuthorities.FirstOrDefault(x => x.Id == userRole.RoleAuthorityId);
        }

        //First authority sets active authority
        public bool AddUserRoles(int userId, IEnumerable<RoleAuthority> authorities)
        {
            bool setAsActive = GetUserRole(userId) == null;

            if(!authorities.Any() &&  setAsActive == false)
            {
                return false;
            }

            foreach(var auth in authorities)
            {
                var role = new UserRole
                {
                    RoleAuthorityId = auth.Id,
                    UserAccountId = userId,
                    ActiveRole = setAsActive
                };

                DbCtx.UserRoles.Add(role);

                setAsActive = false;
            }

            DbCtx.SaveChanges();
            return true;
        }

        //Devices
        public List<NetworkedDevice> GetDevices()
            => DbCtx.NetworkedDevices.ToList();

        public NetworkedDevice? GetDevice(int id) =>
            DbCtx.NetworkedDevices.FirstOrDefault(x => x.Id == id);

        public NetworkedDevice? GetDevice(string deviceId) =>
            DbCtx.NetworkedDevices.FirstOrDefault(x => deviceId.Equals(x.DeviceId, StringComparison.OrdinalIgnoreCase));

        public bool DeleteDevice(int id)
        {
            var device = DbCtx.NetworkedDevices.FirstOrDefault(x => x.Id == id);
            if(device == null) { return false; }

            DbCtx.NetworkedDevices.Remove(device);
            DbCtx.SaveChanges();

            return true;
        }
            
        public bool DeleteDevice(string deviceId)
        {
            var device = GetDevice(deviceId);
            if(device == null) { return false; }

            DbCtx.NetworkedDevices.Remove(device);
            DbCtx.SaveChanges();

            return true;
        }
        
        public bool AddDevice(int userId, IDeviceInfo info)
        {
            var device = DbCtx.NetworkedDevices.FirstOrDefault(x => info.DeviceId.Equals(x.DeviceId, StringComparison.OrdinalIgnoreCase));
            if(device != null)
            {
                return false;
            }
            
            var user = GetUser(userId);
            if(user == null) { return false; }

            device = new NetworkedDevice
            {
                DeviceId = info.DeviceId,
                DeviceName = info.DeviceName,
                OSVersion = info.OsVersion
            };

            var deviceOwner = new NetworkedDeviceOwner
            {
                DeviceId = device.Id,
                OwnerId = user.Id
            };

            DbCtx.NetworkedDevices.Add(device);
            DbCtx.NetworkedDeviceOwners.Add(deviceOwner);
            DbCtx.SaveChanges();

            return true;
        }

        public bool AddDeviceOwner(int userId, int deviceId)
        {
            var user = GetUser(userId);
            var device = GetDevice(deviceId);

            if(user == null || device == null) { return false; }

            var deviceOwner = new NetworkedDeviceOwner
            {
                DeviceId= device.Id,
                OwnerId = user.Id
            };

            DbCtx.NetworkedDeviceOwners.Add(deviceOwner);
            DbCtx.SaveChanges();

            return true;
        }

        public List<NetworkedDeviceOwner> GetDeviceOwners(int deviceId)
            => DbCtx.NetworkedDeviceOwners.Where(x => x.DeviceId == deviceId).ToList();
    }
}
