using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PostgresDb.ZontEntities;

namespace PostgresDb.ZontEntityConfigurations;

public class ZontDeviceConfiguration : IEntityTypeConfiguration<ZontDeviceEntity>
{
    public void Configure(EntityTypeBuilder<ZontDeviceEntity> builder)
    {
        builder.ToTable("zont_devices");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .HasColumnName("id")
            .UseIdentityAlwaysColumn(); // Гарантирует генерацию на стороне БД

        builder.HasIndex(e => e.ZontId)
            .HasDatabaseName("ix_zont_devices_zont_id");

        builder.Property(e => e.ZontId).HasColumnName("zont_id");
        builder.Property(e => e.DeviceId).HasColumnName("device_id").IsRequired();
        builder.Property(e => e.Name).HasColumnName("name").HasMaxLength(255);
        builder.Property(e => e.IsOnline).HasColumnName("is_online");
        builder.Property(e => e.DeviceModel).HasColumnName("device_model");
        builder.Property(e => e.SoftwareVersion).HasColumnName("software_version");
        builder.Property(e => e.HardwareVersion).HasColumnName("hardware_version");
        builder.Property(e => e.FetchedAt)
            .HasColumnName("fetched_at")
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("now()")
            .ValueGeneratedOnAdd();

        foreach (var property in builder.Metadata.GetProperties())
        {
            if (!property.IsPrimaryKey())
            {
                property.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            }
        }
    }
}
