using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OIDC.WebApp.Models;
using OIDC.WebApp.Services;

namespace OIDC.WebApp.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    #region Endpoints

    [HttpGet]
    public async Task<IEnumerable<WeatherForecast>> GetAsync([FromServices] WeatherForecastService service) => await service.GetAsync(User, HttpContext.Request);

    #endregion
}
