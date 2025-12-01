using Weather.Domain.Exceptions;
using Weather.Domain.ValueObjects;

namespace Weather.Domain.Entities;

public class WeatherMeasurement
{
    public Guid Id { get; private set; }
    public DateTime Timestamp { get; private set; }
    public Temperature Temperature { get; private set; }
    public string Conditions { get; private set; }

    private WeatherMeasurement()
    {
        Conditions = string.Empty;
        Temperature = null!;
    }

    public WeatherMeasurement(DateTime timestamp, Temperature temperature, string conditions)
    {
        if (string.IsNullOrWhiteSpace(conditions))
            throw new DomainException("Weather conditions cannot be empty");

        if (conditions.Length > 200)
            throw new DomainException("Weather conditions cannot exceed 200 characters");

        Id = Guid.NewGuid();
        Timestamp = timestamp;
        Temperature = temperature;
        Conditions = conditions.Trim();
    }

    public DateTime GetNormalizedTimestamp()
    {
        return new DateTime(
            Timestamp.Year,
            Timestamp.Month,
            Timestamp.Day,
            Timestamp.Hour,
            (Timestamp.Minute / 10) * 10,
            0,
            Timestamp.Kind
        );
    }
}
