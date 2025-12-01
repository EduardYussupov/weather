using Weather.Domain.Entities;
using Weather.Domain.Exceptions;
using Weather.Domain.ValueObjects;
using Xunit;

namespace Weather.Tests.Domain;

public class CitySubscriptionTests
{
    [Fact]
    public void CreateSubscription_WithIntervalLessThan5Minutes_ThrowsDomainException()
    {
        var cityName = new CityName("London");

        var exception = Assert.Throws<DomainException>(() => new PollingInterval(4));

        Assert.Contains("between 5 and 180", exception.Message);
    }

    [Fact]
    public void CreateSubscription_WithIntervalGreaterThan180Minutes_ThrowsDomainException()
    {
        var cityName = new CityName("London");

        var exception = Assert.Throws<DomainException>(() => new PollingInterval(181));

        Assert.Contains("between 5 and 180", exception.Message);
    }

    [Fact]
    public void CreateSubscription_WithValidInterval_SetsNextPollAtCorrectly()
    {
        var cityName = new CityName("London");
        var pollingInterval = new PollingInterval(30);
        var beforeCreation = DateTime.UtcNow;

        var subscription = new CitySubscription(cityName, pollingInterval);

        var afterCreation = DateTime.UtcNow.AddMinutes(30);
        Assert.True(subscription.NextPollAt >= beforeCreation.AddMinutes(30));
        Assert.True(subscription.NextPollAt <= afterCreation.AddMinutes(1));
        Assert.True(subscription.IsActive);
    }

    [Fact]
    public void UpdatePollingInterval_RecalculatesNextPollAt()
    {
        var cityName = new CityName("London");
        var initialInterval = new PollingInterval(30);
        var subscription = new CitySubscription(cityName, initialInterval);

        var newInterval = new PollingInterval(60);
        var beforeUpdate = DateTime.UtcNow;
        subscription.UpdatePollingInterval(newInterval);

        var expectedTime = beforeUpdate.AddMinutes(60);
        Assert.True(subscription.NextPollAt >= beforeUpdate.AddMinutes(59));
        Assert.True(subscription.NextPollAt <= expectedTime.AddMinutes(1));
    }

    [Fact]
    public void CanDelete_WithRecentMeasurements_ReturnsFalse()
    {
        var cityName = new CityName("London");
        var pollingInterval = new PollingInterval(30);
        var subscription = new CitySubscription(cityName, pollingInterval);

        var recentMeasurement = new WeatherMeasurement(
            DateTime.UtcNow.AddHours(-1),
            new Temperature(20),
            "Clear sky");

        subscription.RegisterMeasurement(recentMeasurement);

        Assert.False(subscription.CanDelete());
    }

    [Fact]
    public void CanDelete_WithoutRecentMeasurements_ReturnsTrue()
    {
        var cityName = new CityName("London");
        var pollingInterval = new PollingInterval(30);
        var subscription = new CitySubscription(cityName, pollingInterval);

        var oldMeasurement = new WeatherMeasurement(
            DateTime.UtcNow.AddDays(-2),
            new Temperature(20),
            "Clear sky");

        subscription.RegisterMeasurement(oldMeasurement);

        Assert.True(subscription.CanDelete());
    }
}
