using Microsoft.AspNetCore.Mvc;
using ecom.Server.Models;
using ecom.Server.ViewModels;

namespace ecom.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherForecastController : ControllerBase
{
    [HttpGet]
    public IEnumerable<WeatherForecastViewModel> Get()
    {
        // Example: Replace with your actual data source
        var forecasts = new List<WeatherForecast>
        {
            new WeatherForecast { Date = DateOnly.FromDateTime(DateTime.Now), TemperatureC = 25, Summary = "Sunny" }
        };

        return forecasts.Select(f => new WeatherForecastViewModel
        {
            Date = f.Date,
            TemperatureC = f.TemperatureC,
            TemperatureF = f.TemperatureF,
            Summary = f.Summary
        });
    }
}
