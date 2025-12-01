using Weather.Domain.Exceptions;
using Weather.Domain.ValueObjects;

namespace Weather.Domain.Entities;

public class CitySubscription
{
    private readonly List<WeatherMeasurement> _measurements = new();

    public Guid Id { get; private set; }
    public CityName CityName { get; private set; }
    public PollingInterval PollingInterval { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime NextPollAt { get; private set; }

    public IReadOnlyCollection<WeatherMeasurement> Measurements => _measurements.AsReadOnly();

    private CitySubscription()
    {
        CityName = null!;
        PollingInterval = null!;
    }

    public CitySubscription(CityName cityName, PollingInterval pollingInterval)
    {
        Id = Guid.NewGuid();
        CityName = cityName;
        PollingInterval = pollingInterval;
        IsActive = true;
        NextPollAt = DateTime.UtcNow.AddMinutes(pollingInterval.Minutes);
    }

    public void UpdatePollingInterval(PollingInterval newInterval)
    {
        PollingInterval = newInterval;
        NextPollAt = DateTime.UtcNow.AddMinutes(newInterval.Minutes);
    }

    public void SetNextPollTime()
    {
        NextPollAt = DateTime.UtcNow.AddMinutes(PollingInterval.Minutes);
    }

    public void Activate()
    {
        IsActive = true;
        SetNextPollTime();
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public bool CanDelete()
    {
        var recentMeasurements = _measurements
            .Where(m => m.Timestamp >= DateTime.UtcNow.AddHours(-24))
            .ToList();

        return !recentMeasurements.Any();
    }

    public void RegisterMeasurement(WeatherMeasurement measurement)
    {
        var normalizedTimestamp = measurement.GetNormalizedTimestamp();

        var existingInSlot = _measurements
            .FirstOrDefault(m => m.GetNormalizedTimestamp() == normalizedTimestamp);

        if (existingInSlot != null)
        {
            _measurements.Remove(existingInSlot);
        }

        _measurements.Add(measurement);
    }
}
