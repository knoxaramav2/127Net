using HomeNet.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeNet.Migrations
{
    public partial class NetHomeDbCtx(DbContextOptions<NetHomeDbCtx> options) 
        : DbContext(options)
    {
        public virtual DbSet<UserAccount> Users { get; set; }
        public virtual DbSet<Device> Devices { get; set; }
        public virtual DbSet<RoleAuthority> Roles { get; set; }
        public virtual DbSet<NetComponent> NetComponents { get; set; }
        public virtual DbSet<NetControl> NetControls { get; set; }
        public virtual DbSet<NetListener> NetListeners { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserAccount>(e => { e.HasKey(k => k.Id); });
            modelBuilder.Entity<RoleAuthority>()
                .HasMany(e => e.MaximumAuthorities)
                .WithOne(e => e.MaximumAuthority)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<RoleAuthority>()
                .HasMany(e => e.OperatingAuthorities)
                .WithOne(e => e.OperatingAuthority)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder builder);
    }
}
