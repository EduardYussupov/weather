using Weather.Domain.Exceptions;

namespace Weather.Domain.ValueObjects;

public sealed record PollingInterval
{
    private const int MinMinutes = 5;
    private const int MaxMinutes = 180;

    public int Minutes { get; }

    public PollingInterval(int minutes)
    {
        if (minutes < MinMinutes || minutes > MaxMinutes)
            throw new DomainException($"Polling interval must be between {MinMinutes} and {MaxMinutes} minutes");

        Minutes = minutes;
    }

    public static implicit operator int(PollingInterval interval) => interval.Minutes;
}
