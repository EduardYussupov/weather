using MediatR;
using Microsoft.AspNetCore.Mvc;
using Weather.Api.Models;
using Weather.Application.Commands.CreateCity;
using Weather.Application.Commands.DeleteCity;
using Weather.Application.Commands.UpdateCity;
using Weather.Application.Queries.GetCities;

namespace Weather.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CitiesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CitiesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<CityDto>>> GetAll(CancellationToken cancellationToken)
    {
        var cities = await _mediator.Send(new GetCitiesQuery(), cancellationToken);
        return Ok(cities);
    }

    [HttpPost]
    public async Task<ActionResult<CreateCityResponse>> Create(
        [FromBody] CreateCityRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateCityCommand(request.CityName, request.PollingIntervalMinutes);
        var cityId = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetAll), new CreateCityResponse(cityId));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Update(
        Guid id,
        [FromBody] UpdateCityRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateCityCommand(id, request.PollingIntervalMinutes, request.IsActive);
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteCityCommand(id), cancellationToken);
        return NoContent();
    }
}
