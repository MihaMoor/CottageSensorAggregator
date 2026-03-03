using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PostgresDb.ZontEntities;

namespace PostgresDb.ZontEntityConfigurations;

public class ZontCircuitsConfiguration : IEntityTypeConfiguration<ZontCircuitsEntity>
{
    public void Configure(EntityTypeBuilder<ZontCircuitsEntity> builder)
    {
        builder.ToTable("zont_circuits");

        // --- Ключи и индексы ---
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .HasColumnName("id")
            .UseIdentityAlwaysColumn();

        // Обычный индекс на ZontId (для связи с устройством)
        builder.HasIndex(e => e.ZontId)
            .HasDatabaseName("ix_zont_circuits_zont_id");

        // --- Маппинг колонок ---
        builder.Property(e => e.ZontId).HasColumnName("zont_id");
        builder.Property(e => e.Name).HasColumnName("name").HasMaxLength(255);
        builder.Property(e => e.Status).HasColumnName("status").HasMaxLength(100);

        // Числовые значения
        builder.Property(e => e.ActualTemp).HasColumnName("actual_temp");
        builder.Property(e => e.CurrentTemp).HasColumnName("current_temp");
        builder.Property(e => e.IsActive).HasColumnName("is_active");
        builder.Property(e => e.Min).HasColumnName("min");
        builder.Property(e => e.Max).HasColumnName("max");
        builder.Property(e => e.Step).HasColumnName("step");

        // Дата: автозаполнение в БД
        builder.Property(e => e.FetchedAt)
            .HasColumnName("fetched_at")
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("now()")
            .ValueGeneratedOnAdd();

        // --- Запрет на любое обновление данных ---
        foreach (var property in builder.Metadata.GetProperties())
        {
            if (!property.IsPrimaryKey())
            {
                property.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            }
        }
    }
}
