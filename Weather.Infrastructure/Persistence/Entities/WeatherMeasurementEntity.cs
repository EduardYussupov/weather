namespace Weather.Infrastructure.Persistence.Entities;

public class WeatherMeasurementEntity
{
    public Guid Id { get; set; }
    public Guid CitySubscriptionId { get; set; }
    public DateTime Timestamp { get; set; }
    public decimal Temperature { get; set; }
    public string Conditions { get; set; } = string.Empty;

    public CitySubscriptionEntity CitySubscription { get; set; } = null!;
}
