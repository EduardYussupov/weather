using MediatR;
using Weather.Domain.Repositories;

namespace Weather.Application.Queries.GetCities;

public class GetCitiesQueryHandler : IRequestHandler<GetCitiesQuery, List<CityDto>>
{
    private readonly ICitySubscriptionRepository _repository;

    public GetCitiesQueryHandler(ICitySubscriptionRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<CityDto>> Handle(GetCitiesQuery request, CancellationToken cancellationToken)
    {
        var subscriptions = await _repository.GetAllAsync(cancellationToken);

        return subscriptions
            .Select(s => new CityDto(
                s.Id,
                s.CityName,
                s.PollingInterval,
                s.IsActive,
                s.NextPollAt))
            .ToList();
    }
}
