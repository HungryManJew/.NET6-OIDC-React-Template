using OIDC.ClassLib.Abstracts;
using OIDC.WebApp.Models;
using OIDC.WebApp.Services;
using System.Security.Claims;
using System.Text.Json;

namespace OIDC.WebApp.Clients
{
    public class WeatherForecastClient : CustomClientBase
    {
        #region DataMembers

        #endregion

        #region Ctor
        public WeatherForecastClient(HttpClient client, ILogger<WeatherForecastClient> logger) : base(client, logger)
        {
        }

        #endregion

        #region Methods

        public async Task<IEnumerable<WeatherForecast>> GetAsync(ClaimsPrincipal user, HttpRequest request)
        {
            try
            {
                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AuthService.GenerateUserToken(user, request));
                var responseMessage = await _client.GetAsync("/weatherforecast");

                if (responseMessage != null)
                {
                    var stream = await responseMessage.Content.ReadAsStreamAsync();
                    return await JsonSerializer.DeserializeAsync<WeatherForecast[]>(stream, options);
                }
                return new WeatherForecast[0];
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        #endregion
    }
}
