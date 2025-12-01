using Weather.Application.Abstractions;

namespace Weather.Infrastructure.Services;

public class StubWeatherProvider : IWeatherProvider
{
    private readonly Random _random = new();

    public Task<WeatherData> GetWeatherAsync(string cityName, CancellationToken cancellationToken = default)
    {
        var temperature = _random.Next(-20, 35) + (_random.NextDouble() * 0.99);
        var conditions = GetRandomConditions();

        return Task.FromResult(new WeatherData((decimal)temperature, conditions));
    }

    private string GetRandomConditions()
    {
        var conditions = new[]
        {
            "Clear sky",
            "Partly cloudy",
            "Cloudy",
            "Overcast",
            "Light rain",
            "Rain",
            "Heavy rain",
            "Thunderstorm",
            "Snow",
            "Fog",
            "Windy"
        };

        return conditions[_random.Next(conditions.Length)];
    }
}
