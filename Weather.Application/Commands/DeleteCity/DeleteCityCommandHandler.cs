using MediatR;
using Weather.Domain.Repositories;

namespace Weather.Application.Commands.DeleteCity;

public class DeleteCityCommandHandler : IRequestHandler<DeleteCityCommand>
{
    private readonly ICitySubscriptionRepository _repository;

    public DeleteCityCommandHandler(ICitySubscriptionRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteCityCommand request, CancellationToken cancellationToken)
    {
        var subscription = await _repository.GetByIdAsync(request.CityId, cancellationToken)
            ?? throw new InvalidOperationException($"City subscription {request.CityId} not found");

        if (subscription.CanDelete())
        {
            await _repository.DeleteAsync(subscription, cancellationToken);
        }
        else
        {
            subscription.Deactivate();
            await _repository.UpdateAsync(subscription, cancellationToken);
        }
    }
}
