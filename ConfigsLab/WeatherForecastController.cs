using ConfigsLab;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace YourProjectName.Controllers
{
    [ApiController]
    //[Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly WeatherApiOptions _weatherApiOptions;
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild",
            "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, WeatherApiOptions weatherApiOptions)
        {
            _logger = logger;
            _weatherApiOptions = weatherApiOptions;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
        [HttpGet("config")]
        public IActionResult GetConfig()
        {
            return Ok(new
            {
                BaseUrl = _weatherApiOptions.BaseUrl,
                TimeoutSeconds = _weatherApiOptions.TimeoutSeconds,
                EnabledCashing = _weatherApiOptions.EnabledCashing,
                HasApiKey = !string.IsNullOrEmpty(_weatherApiOptions.ApiKey)
            });
        }
    }
}