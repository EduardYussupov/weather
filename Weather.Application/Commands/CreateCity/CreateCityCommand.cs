using MediatR;

namespace Weather.Application.Commands.CreateCity;

public record CreateCityCommand(string CityName, int PollingIntervalMinutes) : IRequest<Guid>;
