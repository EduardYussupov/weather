using Weather.Domain.Exceptions;

namespace Weather.Domain.ValueObjects;

public sealed record Temperature
{
    private const decimal MinCelsius = -90m;
    private const decimal MaxCelsius = 60m;

    public decimal Celsius { get; }

    public Temperature(decimal celsius)
    {
        if (celsius < MinCelsius || celsius > MaxCelsius)
            throw new DomainException($"Temperature must be between {MinCelsius} and {MaxCelsius} degrees Celsius");

        Celsius = celsius;
    }

    public static implicit operator decimal(Temperature temperature) => temperature.Celsius;
}
