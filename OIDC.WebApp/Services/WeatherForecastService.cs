using AutoMapper;
using OIDC.ClassLib.Abstracts;
using OIDC.WebApp.Clients;
using OIDC.WebApp.Models;
using System.Security.Claims;

namespace OIDC.WebApp.Services
{
    public class WeatherForecastService : CustomServiceBase<WeatherForecast>
    {
        #region DataMembers

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        #endregion

        #region Ctor

        public WeatherForecastService(ILogger<WeatherForecastService> logger, IMapper mapper, WeatherForecastClient client) : base(logger, mapper, client) { }

        #endregion

        #region Methods

        public async Task<IEnumerable<WeatherForecast>> GetAsync(ClaimsPrincipal user, HttpRequest request)
        {
            return await ((WeatherForecastClient)_client).GetAsync(user, request);
        }

        #endregion

    }

}
