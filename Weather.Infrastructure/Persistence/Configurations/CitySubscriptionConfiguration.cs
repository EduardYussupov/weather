using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Weather.Infrastructure.Persistence.Entities;

namespace Weather.Infrastructure.Persistence.Configurations;

public class CitySubscriptionConfiguration : IEntityTypeConfiguration<CitySubscriptionEntity>
{
    public void Configure(EntityTypeBuilder<CitySubscriptionEntity> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.CityName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.PollingIntervalMinutes)
            .IsRequired();

        builder.Property(e => e.IsActive)
            .IsRequired();

        builder.Property(e => e.NextPollAt)
            .IsRequired();

        builder.HasMany(e => e.Measurements)
            .WithOne(m => m.CitySubscription)
            .HasForeignKey(m => m.CitySubscriptionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.CityName);
        builder.HasIndex(e => new { e.IsActive, e.NextPollAt });
    }
}
