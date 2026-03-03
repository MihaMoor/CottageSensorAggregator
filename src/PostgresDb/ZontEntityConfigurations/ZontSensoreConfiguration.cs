using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PostgresDb.ZontEntities;

namespace PostgresDb.ZontEntityConfigurations;

public class ZontSensoreConfiguration : IEntityTypeConfiguration<ZontSensoreEntity>
{
    public void Configure(EntityTypeBuilder<ZontSensoreEntity> builder)
    {
        // Имя таблицы
        builder.ToTable("zont_sensors");

        // --- Ключи и индексы ---
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .HasColumnName("id")
            .UseIdentityAlwaysColumn();

        // Индекс для быстрой фильтрации по ID системы Zont
        builder.HasIndex(e => e.ZontId)
            .HasDatabaseName("ix_zont_sensors_zont_id");

        // --- Маппинг колонок ---
        builder.Property(e => e.ZontId).HasColumnName("zont_id");

        builder.Property(e => e.Name)
            .HasColumnName("name")
            .HasMaxLength(255);

        builder.Property(e => e.Type)
            .HasColumnName("type")
            .HasMaxLength(100);

        builder.Property(e => e.Status)
            .HasColumnName("status")
            .HasMaxLength(100);

        builder.Property(e => e.Value)
            .HasColumnName("value");

        builder.Property(e => e.Unit)
            .HasColumnName("unit")
            .HasMaxLength(20);

        // Дата: автозаполнение в БД
        builder.Property(e => e.FetchedAt)
            .HasColumnName("fetched_at")
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("now()")
            .ValueGeneratedOnAdd();

        // --- Запрет на обновление данных (Immutable) ---
        foreach (var property in builder.Metadata.GetProperties())
        {
            if (!property.IsPrimaryKey())
            {
                property.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            }
        }
    }
}
