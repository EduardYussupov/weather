using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Weather.Infrastructure.Persistence.Entities;

namespace Weather.Infrastructure.Persistence.Configurations;

public class WeatherMeasurementConfiguration : IEntityTypeConfiguration<WeatherMeasurementEntity>
{
    public void Configure(EntityTypeBuilder<WeatherMeasurementEntity> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.CitySubscriptionId)
            .IsRequired();

        builder.Property(e => e.Timestamp)
            .IsRequired();

        builder.Property(e => e.Temperature)
            .IsRequired()
            .HasPrecision(5, 2);

        builder.Property(e => e.Conditions)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(e => new { e.CitySubscriptionId, e.Timestamp });
    }
}
