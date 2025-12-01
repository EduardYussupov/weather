using Microsoft.EntityFrameworkCore;
using Weather.Infrastructure.Persistence.Configurations;
using Weather.Infrastructure.Persistence.Entities;

namespace Weather.Infrastructure.Persistence;

public class WeatherDbContext : DbContext
{
    public WeatherDbContext(DbContextOptions<WeatherDbContext> options) : base(options)
    {
    }

    public DbSet<CitySubscriptionEntity> CitySubscriptions { get; set; }
    public DbSet<WeatherMeasurementEntity> WeatherMeasurements { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CitySubscriptionConfiguration());
        modelBuilder.ApplyConfiguration(new WeatherMeasurementConfiguration());
    }
}
