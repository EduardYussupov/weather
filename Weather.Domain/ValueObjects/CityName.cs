using Weather.Domain.Exceptions;

namespace Weather.Domain.ValueObjects;

public sealed record CityName
{
    private const int MaxLength = 100;

    public string Value { get; }

    public CityName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("City name cannot be empty");

        var normalized = value.Trim();
        if (normalized.Length > MaxLength)
            throw new DomainException($"City name cannot exceed {MaxLength} characters");

        Value = normalized;
    }

    public static implicit operator string(CityName cityName) => cityName.Value;
}
