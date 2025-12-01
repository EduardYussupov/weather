using Weather.Domain.Entities;
using Weather.Domain.Exceptions;
using Weather.Domain.ValueObjects;
using Xunit;

namespace Weather.Tests.Domain;

public class WeatherMeasurementTests
{
    [Fact]
    public void RegisterMeasurement_InSame10MinuteSlot_KeepsOnlyLastMeasurement()
    {
        var cityName = new CityName("London");
        var pollingInterval = new PollingInterval(30);
        var subscription = new CitySubscription(cityName, pollingInterval);

        var baseTime = new DateTime(2024, 1, 1, 12, 5, 0, DateTimeKind.Utc);
        var firstMeasurement = new WeatherMeasurement(baseTime, new Temperature(20), "Sunny");
        var secondMeasurement = new WeatherMeasurement(baseTime.AddMinutes(3), new Temperature(22), "Cloudy");

        subscription.RegisterMeasurement(firstMeasurement);
        subscription.RegisterMeasurement(secondMeasurement);

        Assert.Single(subscription.Measurements);
        Assert.Equal(22m, subscription.Measurements.First().Temperature.Celsius);
        Assert.Equal("Cloudy", subscription.Measurements.First().Conditions);
    }

    [Fact]
    public void CreateTemperature_WithValueAbove60_ThrowsDomainException()
    {
        var exception = Assert.Throws<DomainException>(() => new Temperature(61));

        Assert.Contains("between -90 and 60", exception.Message);
    }

    [Fact]
    public void CreateTemperature_WithValueBelow90_ThrowsDomainException()
    {
        var exception = Assert.Throws<DomainException>(() => new Temperature(-91));

        Assert.Contains("between -90 and 60", exception.Message);
    }

    [Fact]
    public void CreateTemperature_WithValidValue_CreatesSuccessfully()
    {
        var temperature = new Temperature(25.5m);

        Assert.Equal(25.5m, temperature.Celsius);
    }

    [Fact]
    public void GetNormalizedTimestamp_RoundsToNearest10Minutes()
    {
        var timestamp = new DateTime(2024, 1, 1, 12, 37, 45, DateTimeKind.Utc);
        var measurement = new WeatherMeasurement(timestamp, new Temperature(20), "Sunny");

        var normalized = measurement.GetNormalizedTimestamp();

        Assert.Equal(new DateTime(2024, 1, 1, 12, 30, 0, DateTimeKind.Utc), normalized);
    }
}
