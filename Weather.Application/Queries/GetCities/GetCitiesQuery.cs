using MediatR;

namespace Weather.Application.Queries.GetCities;

public record GetCitiesQuery : IRequest<List<CityDto>>;

public record CityDto(
    Guid Id,
    string CityName,
    int PollingIntervalMinutes,
    bool IsActive,
    DateTime NextPollAt);
