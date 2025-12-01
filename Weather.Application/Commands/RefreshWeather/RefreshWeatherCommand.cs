using MediatR;

namespace Weather.Application.Commands.RefreshWeather;

public record RefreshWeatherCommand(Guid CityId) : IRequest;
