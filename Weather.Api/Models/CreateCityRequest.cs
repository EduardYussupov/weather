namespace Weather.Api.Models;

public record CreateCityRequest(string CityName, int PollingIntervalMinutes);

public record CreateCityResponse(Guid CityId);
