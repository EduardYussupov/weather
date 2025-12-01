using MediatR;
using Microsoft.AspNetCore.Mvc;
using Weather.Application.Queries.GetWeatherHistory;

namespace Weather.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherController : ControllerBase
{
    private readonly IMediator _mediator;

    public WeatherController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{cityId:guid}/history")]
    public async Task<ActionResult<WeatherHistoryResponse>> GetHistory(
        Guid cityId,
        [FromQuery] DateTime from,
        [FromQuery] DateTime to,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50,
        CancellationToken cancellationToken = default)
    {
        var query = new GetWeatherHistoryQuery(cityId, from, to, page, pageSize);
        var response = await _mediator.Send(query, cancellationToken);

        return Ok(response);
    }
}
