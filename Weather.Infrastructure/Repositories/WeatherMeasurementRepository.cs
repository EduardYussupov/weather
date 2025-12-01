using Microsoft.EntityFrameworkCore;
using Weather.Domain.Entities;
using Weather.Domain.Repositories;
using Weather.Domain.ValueObjects;
using Weather.Infrastructure.Persistence;
using Weather.Infrastructure.Persistence.Entities;

namespace Weather.Infrastructure.Repositories;

public class WeatherMeasurementRepository : IWeatherMeasurementRepository
{
    private readonly WeatherDbContext _context;

    public WeatherMeasurementRepository(WeatherDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<WeatherMeasurement>> GetHistoryAsync(
        Guid cityId,
        DateTime from,
        DateTime to,
        int skip,
        int take,
        CancellationToken cancellationToken = default)
    {
        var entities = await _context.WeatherMeasurements
            .AsNoTracking()
            .Where(m => m.CitySubscriptionId == cityId && m.Timestamp >= from && m.Timestamp <= to)
            .OrderByDescending(m => m.Timestamp)
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<int> GetHistoryCountAsync(
        Guid cityId,
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken = default)
    {
        return await _context.WeatherMeasurements
            .Where(m => m.CitySubscriptionId == cityId && m.Timestamp >= from && m.Timestamp <= to)
            .CountAsync(cancellationToken);
    }

    public async Task AddAsync(WeatherMeasurement measurement, Guid cityId, CancellationToken cancellationToken = default)
    {
        var entity = new WeatherMeasurementEntity
        {
            Id = measurement.Id,
            CitySubscriptionId = cityId,
            Timestamp = measurement.Timestamp,
            Temperature = measurement.Temperature,
            Conditions = measurement.Conditions
        };

        await _context.WeatherMeasurements.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    private static WeatherMeasurement MapToDomain(WeatherMeasurementEntity entity)
    {
        var temperature = new Temperature(entity.Temperature);
        return new WeatherMeasurement(entity.Timestamp, temperature, entity.Conditions);
    }
}
