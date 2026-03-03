using Microsoft.EntityFrameworkCore;
using PostgresDb.ZontEntities;
using PostgresDb.ZontEntityConfigurations;

namespace PostgresDb;

public class PostgresContext : DbContext
{
    public DbSet<ZontDeviceEntity> ZontDevices { get; set; }
    public DbSet<ZontCircuitsEntity> ZontCircuits { get; set; }
    public DbSet<ZontSensoreEntity> ZontSensores { get; set; }

    public PostgresContext(DbContextOptions<PostgresContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new ZontDeviceConfiguration());
        modelBuilder.ApplyConfiguration(new ZontCircuitsConfiguration());
        modelBuilder.ApplyConfiguration(new ZontSensoreConfiguration());
    }
}
