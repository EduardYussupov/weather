using MediatR;

namespace Weather.Application.Commands.DeleteCity;

public record DeleteCityCommand(Guid CityId) : IRequest;
