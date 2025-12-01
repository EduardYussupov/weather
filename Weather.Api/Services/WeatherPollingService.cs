using MediatR;
using Weather.Application.Commands.RefreshWeather;
using Weather.Domain.Repositories;

namespace Weather.Api.Services;

public class WeatherPollingService : BackgroundService
{
    private readonly ILogger<WeatherPollingService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public WeatherPollingService(ILogger<WeatherPollingService> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Weather Polling Service started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessWeatherPollingAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing weather polling");
            }

            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }

    private async Task ProcessWeatherPollingAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<ICitySubscriptionRepository>();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var citiesDueForPolling = await repository.GetActiveDueForPollingAsync(cancellationToken);

        if (!citiesDueForPolling.Any())
            return;

        _logger.LogInformation("Found {Count} cities due for polling", citiesDueForPolling.Count);

        foreach (var city in citiesDueForPolling)
        {
            try
            {
                await mediator.Send(new RefreshWeatherCommand(city.Id), cancellationToken);
                _logger.LogInformation("Weather data refreshed for city {CityName}", city.CityName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to refresh weather for city {CityName}", city.CityName);
            }
        }
    }
}
