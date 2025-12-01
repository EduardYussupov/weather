using Weather.Domain.Entities;

namespace Weather.Domain.Repositories;

public interface ICitySubscriptionRepository
{
    Task<CitySubscription?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CitySubscription>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CitySubscription>> GetActiveDueForPollingAsync(CancellationToken cancellationToken = default);
    Task AddAsync(CitySubscription subscription, CancellationToken cancellationToken = default);
    Task UpdateAsync(CitySubscription subscription, CancellationToken cancellationToken = default);
    Task DeleteAsync(CitySubscription subscription, CancellationToken cancellationToken = default);
}
