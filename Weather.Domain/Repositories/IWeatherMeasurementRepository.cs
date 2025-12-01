using Weather.Domain.Entities;

namespace Weather.Domain.Repositories;

public interface IWeatherMeasurementRepository
{
    Task<IReadOnlyList<WeatherMeasurement>> GetHistoryAsync(
        Guid cityId,
        DateTime from,
        DateTime to,
        int skip,
        int take,
        CancellationToken cancellationToken = default);

    Task<int> GetHistoryCountAsync(
        Guid cityId,
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken = default);

    Task AddAsync(WeatherMeasurement measurement, Guid cityId, CancellationToken cancellationToken = default);
}
