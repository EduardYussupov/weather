namespace Weather.Api.Models;

public record UpdateCityRequest(int? PollingIntervalMinutes, bool? IsActive);
