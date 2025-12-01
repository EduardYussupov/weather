namespace Weather.Application.Abstractions;

public interface IWeatherProvider
{
    Task<WeatherData> GetWeatherAsync(string cityName, CancellationToken cancellationToken = default);
}

public record WeatherData(decimal Temperature, string Conditions);
