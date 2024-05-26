using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace HomeCore.Data
{
    public class HomeCoreDbCtx(DbContextOptions<HomeCoreDbCtx> options) 
        : IdentityDbContext<UserAccount>(options)
    {
        public virtual DbSet<UserAccount> UserAccounts { get; set; }
        public virtual DbSet<Device> Devices { get; set; }
        public virtual DbSet<RoleAuthority> AuthRoles { get; set; }
        public virtual DbSet<NetComponent> NetComponents { get; set; }
        public virtual DbSet<NetControl> NetControls { get; set; }
        public virtual DbSet<NetListener> NetListeners { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserAccount>(e => { e.HasKey(k => k.Id); });
            builder.Entity<RoleAuthority>(e => { e.HasKey(k => k.Id); });

            builder.Entity<RoleAuthority>()
                .HasMany(e => e.OperatingAuthorities)
                .WithOne(e => e.OperatingAuthority)
                .HasForeignKey(e => e.RoleAuthorityId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}
