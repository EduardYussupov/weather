namespace Weather.Infrastructure.Persistence.Entities;

public class CitySubscriptionEntity
{
    public Guid Id { get; set; }
    public string CityName { get; set; } = string.Empty;
    public int PollingIntervalMinutes { get; set; }
    public bool IsActive { get; set; }
    public DateTime NextPollAt { get; set; }

    public List<WeatherMeasurementEntity> Measurements { get; set; } = new();
}
