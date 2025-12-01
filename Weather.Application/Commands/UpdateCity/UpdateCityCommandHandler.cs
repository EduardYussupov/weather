using MediatR;
using Weather.Domain.Repositories;
using Weather.Domain.ValueObjects;

namespace Weather.Application.Commands.UpdateCity;

public class UpdateCityCommandHandler : IRequestHandler<UpdateCityCommand>
{
    private readonly ICitySubscriptionRepository _repository;

    public UpdateCityCommandHandler(ICitySubscriptionRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(UpdateCityCommand request, CancellationToken cancellationToken)
    {
        var subscription = await _repository.GetByIdAsync(request.CityId, cancellationToken)
            ?? throw new InvalidOperationException($"City subscription {request.CityId} not found");

        if (request.PollingIntervalMinutes.HasValue)
        {
            var newInterval = new PollingInterval(request.PollingIntervalMinutes.Value);
            subscription.UpdatePollingInterval(newInterval);
        }

        if (request.IsActive.HasValue)
        {
            if (request.IsActive.Value)
                subscription.Activate();
            else
                subscription.Deactivate();
        }

        await _repository.UpdateAsync(subscription, cancellationToken);
    }
}
