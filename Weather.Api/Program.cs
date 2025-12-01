using Microsoft.EntityFrameworkCore;
using Weather.Api.Services;
using Weather.Application.Abstractions;
using Weather.Domain.Repositories;
using Weather.Infrastructure.Persistence;
using Weather.Infrastructure.Repositories;
using Weather.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Weather.Application.Commands.CreateCity.CreateCityCommand).Assembly));

builder.Services.AddDbContext<WeatherDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ICitySubscriptionRepository, CitySubscriptionRepository>();
builder.Services.AddScoped<IWeatherMeasurementRepository, WeatherMeasurementRepository>();
builder.Services.AddScoped<IWeatherProvider, StubWeatherProvider>();

builder.Services.AddHostedService<WeatherPollingService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
