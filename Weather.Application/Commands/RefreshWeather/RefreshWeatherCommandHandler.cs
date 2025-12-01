using MediatR;
using Weather.Application.Abstractions;
using Weather.Domain.Entities;
using Weather.Domain.Repositories;
using Weather.Domain.ValueObjects;

namespace Weather.Application.Commands.RefreshWeather;

public class RefreshWeatherCommandHandler : IRequestHandler<RefreshWeatherCommand>
{
    private readonly ICitySubscriptionRepository _cityRepository;
    private readonly IWeatherMeasurementRepository _measurementRepository;
    private readonly IWeatherProvider _weatherProvider;

    public RefreshWeatherCommandHandler(
        ICitySubscriptionRepository cityRepository,
        IWeatherMeasurementRepository measurementRepository,
        IWeatherProvider weatherProvider)
    {
        _cityRepository = cityRepository;
        _measurementRepository = measurementRepository;
        _weatherProvider = weatherProvider;
    }

    public async Task Handle(RefreshWeatherCommand request, CancellationToken cancellationToken)
    {
        var subscription = await _cityRepository.GetByIdAsync(request.CityId, cancellationToken)
            ?? throw new InvalidOperationException($"City subscription {request.CityId} not found");

        var weatherData = await _weatherProvider.GetWeatherAsync(subscription.CityName, cancellationToken);

        var temperature = new Temperature(weatherData.Temperature);
        var measurement = new WeatherMeasurement(DateTime.UtcNow, temperature, weatherData.Conditions);

        subscription.RegisterMeasurement(measurement);
        subscription.SetNextPollTime();

        await _measurementRepository.AddAsync(measurement, subscription.Id, cancellationToken);
        await _cityRepository.UpdateAsync(subscription, cancellationToken);
    }
}
