using MediatR;

namespace Weather.Application.Queries.GetWeatherHistory;

public record GetWeatherHistoryQuery(
    Guid CityId,
    DateTime From,
    DateTime To,
    int Page = 1,
    int PageSize = 50) : IRequest<WeatherHistoryResponse>;

public record WeatherHistoryResponse(
    List<WeatherMeasurementDto> Measurements,
    int TotalCount,
    int Page,
    int PageSize);

public record WeatherMeasurementDto(
    Guid Id,
    DateTime Timestamp,
    decimal Temperature,
    string Conditions);
