using MediatR;
using Weather.Domain.Repositories;

namespace Weather.Application.Queries.GetWeatherHistory;

public class GetWeatherHistoryQueryHandler : IRequestHandler<GetWeatherHistoryQuery, WeatherHistoryResponse>
{
    private readonly IWeatherMeasurementRepository _repository;

    public GetWeatherHistoryQueryHandler(IWeatherMeasurementRepository repository)
    {
        _repository = repository;
    }

    public async Task<WeatherHistoryResponse> Handle(GetWeatherHistoryQuery request, CancellationToken cancellationToken)
    {
        var skip = (request.Page - 1) * request.PageSize;

        var measurements = await _repository.GetHistoryAsync(
            request.CityId,
            request.From,
            request.To,
            skip,
            request.PageSize,
            cancellationToken);

        var totalCount = await _repository.GetHistoryCountAsync(
            request.CityId,
            request.From,
            request.To,
            cancellationToken);

        var dtos = measurements
            .Select(m => new WeatherMeasurementDto(
                m.Id,
                m.Timestamp,
                m.Temperature,
                m.Conditions))
            .ToList();

        return new WeatherHistoryResponse(dtos, totalCount, request.Page, request.PageSize);
    }
}
