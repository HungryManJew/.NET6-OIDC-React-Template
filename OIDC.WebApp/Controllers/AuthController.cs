using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OIDC.WebApp.Services;

namespace OIDC.WebApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        #region Endpoints

        [HttpGet("[action]")]
        public IActionResult Login([FromServices] AuthService service, string? RedirectUri = "/") =>
            service.Login(RedirectUri);

        [HttpGet("claims")]
        public IActionResult GetUserClaims([FromServices] AuthService service) =>
            service.GetUserClaims(User);

        [HttpGet("token")]
        public IActionResult GetUserToken([FromServices] AuthService service) =>
            service.GetUserToken(User);

        #endregion
    }
}
