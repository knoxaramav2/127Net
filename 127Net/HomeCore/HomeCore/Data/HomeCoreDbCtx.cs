using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using MySql.EntityFrameworkCore.DataAnnotations;
using System.Diagnostics;
using System.Reflection.Emit;

namespace HomeCore.Data
{
    public class HomeCoreDbCtx(DbContextOptions<HomeCoreDbCtx> options) 
        : IdentityDbContext<UserAccount>(options)
    {
        public virtual DbSet<UserAccount> UserAccounts { get; set; }
        public virtual DbSet<Device> Devices { get; set; }
        public virtual DbSet<DeviceOwner> DeviceOwners { get; set; }
        public virtual DbSet<RoleAuthority> AuthRoles { get; set; }
        public virtual DbSet<NetComponent> NetComponents { get; set; }
        public virtual DbSet<NetControl> NetControls { get; set; }
        public virtual DbSet<NetListener> NetListeners { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Debugger.Launch();
            builder.Entity<UserAccount>(e => { e.HasKey(k => k.Id); });
            builder.Entity<DeviceOwner>(e => { e.HasKey(k => k.Id); });
            builder.Entity<RoleAuthority>(e => { e.HasKey(k => k.Id); });
            builder.Entity<Device>(e => { e.HasKey(k => k.Id); });

            builder.Entity<ConnectedDevice>(e =>
            {
                e.HasKey(x => new { x.Device1Id, x.Device2Id });

                e.HasOne(x => x.Device1)
                .WithMany(x => x.Connected)
                .HasForeignKey(x => x.Device1Id)
                .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.Device2)
                .WithMany()
                .HasForeignKey(x => x.Device2Id)
                .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<RoleAuthority>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasMany(x => x.MaximumAuthorities)
                .WithOne(x => x.MaxAuthority)
                .HasForeignKey(x => x.MaxAuthorityId)
                .OnDelete(DeleteBehavior.Restrict);

                e.HasKey(x => x.Id);
                e.HasMany(x => x.OperatingAuthorities)
                .WithOne(x => x.OperatingAuthority)
                .HasForeignKey(x => x.OperatingAuthorityId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<UserAccount>(e =>
            {
                e.HasKey(e => e.Id);
                e.HasOne(x => x.MaxAuthority)
                .WithMany(x => x.MaximumAuthorities)
                .HasForeignKey(x => x.MaxAuthorityId)
                .OnDelete(DeleteBehavior.Restrict);

                e.HasKey(e => e.Id);
                e.HasOne(x => x.OperatingAuthority)
                .WithMany(x => x.OperatingAuthorities)
                .HasForeignKey(x => x.OperatingAuthorityId)
                .OnDelete(DeleteBehavior.Restrict);
            });


            RoleAuthority[] auths = [
                new RoleAuthority{AuthLevel=0, ReauthTime=30, RoleName="Admin", Id=-1},
                new RoleAuthority{AuthLevel=5, ReauthTime=300, RoleName="Owner", Id=-2},
                new RoleAuthority{AuthLevel=10, ReauthTime=3600, RoleName="Guest", Id=-3}
                ];

            builder.Entity<RoleAuthority>().HasData(auths);

            base.OnModelCreating(builder);
        }
    }
}
