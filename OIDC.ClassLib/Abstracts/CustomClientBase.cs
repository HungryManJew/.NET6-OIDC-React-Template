using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace OIDC.ClassLib.Abstracts
{
    public abstract class CustomClientBase
    {
        #region DataMembers

        protected readonly JsonSerializerOptions options = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        protected readonly HttpClient _client;
        protected readonly ILogger<CustomClientBase> _logger;

        #endregion

        #region Ctor

        public CustomClientBase(HttpClient client, ILogger<CustomClientBase> logger)
        {
            _client = client;
            _logger = logger;
        }

        #endregion

    }
}
