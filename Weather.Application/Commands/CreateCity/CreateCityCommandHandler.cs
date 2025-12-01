using MediatR;
using Weather.Domain.Entities;
using Weather.Domain.Repositories;
using Weather.Domain.ValueObjects;

namespace Weather.Application.Commands.CreateCity;

public class CreateCityCommandHandler : IRequestHandler<CreateCityCommand, Guid>
{
    private readonly ICitySubscriptionRepository _repository;

    public CreateCityCommandHandler(ICitySubscriptionRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateCityCommand request, CancellationToken cancellationToken)
    {
        var cityName = new CityName(request.CityName);
        var pollingInterval = new PollingInterval(request.PollingIntervalMinutes);

        var subscription = new CitySubscription(cityName, pollingInterval);

        await _repository.AddAsync(subscription, cancellationToken);

        return subscription.Id;
    }
}
