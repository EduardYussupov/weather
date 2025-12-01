using MediatR;

namespace Weather.Application.Commands.UpdateCity;

public record UpdateCityCommand(
    Guid CityId,
    int? PollingIntervalMinutes,
    bool? IsActive) : IRequest;
