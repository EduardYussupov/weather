using Microsoft.EntityFrameworkCore;
using Weather.Domain.Entities;
using Weather.Domain.Repositories;
using Weather.Domain.ValueObjects;
using Weather.Infrastructure.Persistence;
using Weather.Infrastructure.Persistence.Entities;

namespace Weather.Infrastructure.Repositories;

public class CitySubscriptionRepository : ICitySubscriptionRepository
{
    private readonly WeatherDbContext _context;

    public CitySubscriptionRepository(WeatherDbContext context)
    {
        _context = context;
    }

    public async Task<CitySubscription?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.CitySubscriptions
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        return entity == null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyList<CitySubscription>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _context.CitySubscriptions
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IReadOnlyList<CitySubscription>> GetActiveDueForPollingAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var entities = await _context.CitySubscriptions
            .AsNoTracking()
            .Where(c => c.IsActive && c.NextPollAt <= now)
            .ToListAsync(cancellationToken);

        return entities.Select(MapToDomain).ToList();
    }

    public async Task AddAsync(CitySubscription subscription, CancellationToken cancellationToken = default)
    {
        var entity = MapToEntity(subscription);
        await _context.CitySubscriptions.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(CitySubscription subscription, CancellationToken cancellationToken = default)
    {
        var entity = await _context.CitySubscriptions
            .FirstOrDefaultAsync(c => c.Id == subscription.Id, cancellationToken);

        if (entity == null)
            throw new InvalidOperationException($"City subscription {subscription.Id} not found");

        entity.CityName = subscription.CityName;
        entity.PollingIntervalMinutes = subscription.PollingInterval;
        entity.IsActive = subscription.IsActive;
        entity.NextPollAt = subscription.NextPollAt;

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(CitySubscription subscription, CancellationToken cancellationToken = default)
    {
        var entity = await _context.CitySubscriptions
            .FirstOrDefaultAsync(c => c.Id == subscription.Id, cancellationToken);

        if (entity != null)
        {
            _context.CitySubscriptions.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    private static CitySubscription MapToDomain(CitySubscriptionEntity entity)
    {
        var cityName = new CityName(entity.CityName);
        var pollingInterval = new PollingInterval(entity.PollingIntervalMinutes);
        
        var subscription = new CitySubscription(cityName, pollingInterval);

        typeof(CitySubscription).GetProperty(nameof(CitySubscription.Id))!
            .SetValue(subscription, entity.Id);
        typeof(CitySubscription).GetProperty(nameof(CitySubscription.IsActive))!
            .SetValue(subscription, entity.IsActive);
        typeof(CitySubscription).GetProperty(nameof(CitySubscription.NextPollAt))!
            .SetValue(subscription, entity.NextPollAt);

        return subscription;
    }

    private static CitySubscriptionEntity MapToEntity(CitySubscription subscription)
    {
        return new CitySubscriptionEntity
        {
            Id = subscription.Id,
            CityName = subscription.CityName,
            PollingIntervalMinutes = subscription.PollingInterval,
            IsActive = subscription.IsActive,
            NextPollAt = subscription.NextPollAt
        };
    }
}
