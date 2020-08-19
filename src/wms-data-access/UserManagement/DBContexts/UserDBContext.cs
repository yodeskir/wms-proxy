using Microsoft.EntityFrameworkCore;
using wmsDataAccess.UserManagement.Entities;
using WMSDataAccess.UserManagement.Entities;

namespace WMSDataAccess.UserManagement.DBContexts
{
    public class UserDBContext: DbContext
    {
        public UserDBContext()
        {

        }

        public UserDBContext(DbContextOptions<UserDBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                var currentTableName = modelBuilder.Entity(entity.Name).Metadata.GetTableName();
                modelBuilder.Entity(entity.Name).ToTable(currentTableName.ToLower());
            }
            modelBuilder.HasPostgresExtension("POSTGIS");
            modelBuilder.HasDefaultSchema("public");
            modelBuilder.Entity<WMSUser>().HasKey(p => new { p.username });
            modelBuilder.Entity<WMSUser>().Property(b => b.creationdate).HasDefaultValueSql("CURRENT_DATE");
            modelBuilder.Entity<WMSUser>().HasMany(m => m.maps).WithOne();
            modelBuilder.Entity<WMSMaps>().HasKey(m => m.id);
            modelBuilder.Entity<WMSMapsLog>().HasKey(m => m.id);
        }
        public DbSet<WMSUser> Users { get; set; }
        public DbSet<WMSMaps> Maps { get; set; }
        public DbSet<WMSMapsLog> MapsLog { get; set; }
        public DbSet<WMSLayers> wmsLayers { get; set; }
    }
}
