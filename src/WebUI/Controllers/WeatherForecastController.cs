using PolizaWebAPI.Application.WeatherForecasts.Queries.GetWeatherForecasts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebUI.Filters;

namespace PolizaWebAPI.WebUI.Controllers;

public class WeatherForecastController : ApiControllerBase
{


    [HttpGet]
    [SpecialAuthorizeAttribute(Feature = "WEATHER")]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {
        return await Mediator.Send(new GetWeatherForecastsQuery());
    }
}
