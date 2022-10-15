using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OIDC.ClassLib.Abstracts;
using OIDC.WebApp.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace OIDC.WebApp.Services
{
    public class AuthService : CustomServiceBase<ClaimsPrincipal>
    {
        #region DataMembers

        #endregion

        #region Ctor

        public AuthService(ILogger<AuthService> logger, IMapper mapper) : base(logger, mapper) { }

        #endregion

        #region Methods

        public IActionResult Login(string RedirectUri = "/")
        {
            return Redirect(RedirectUri);
        }

        public IActionResult GetUserClaims(ClaimsPrincipal user)
        {
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(user.Identity);
            List<Claim> claimsRes = claimsPrincipal.Claims.ToList();
            return Ok(_mapper.Map<List<Claim>, List<UserClaimsDto>>(claimsRes));
        }

        public IActionResult GetUserToken(ClaimsPrincipal user)
        {
            return Ok(GenerateUserToken(user, HttpContext.Request));
        }

        #endregion

        #region StaticMethods

        public static string GenerateUserToken(ClaimsPrincipal user, HttpRequest request)
        {
            var issuer = Program.configuration["JwtConfig:Authority"];
            var audience = string.Format("{0}://{1}", request.Scheme, request.Host);
            using (RSA rsa = RSA.Create())
            {
                rsa.ImportRSAPrivateKey( // Convert the loaded key from base64 to bytes.
                    source: Convert.FromBase64String(Program.configuration["JwtConfig:PrivateKey"]), // Use the private key to sign tokens
                    bytesRead: out int _); // Discard the out variable 

                var signingCredentials = new SigningCredentials(
                    key: new RsaSecurityKey(rsa),
                    algorithm: SecurityAlgorithms.RsaSha256 // Important to use RSA version of the SHA algo 
                )
                {
                    CryptoProviderFactory = new CryptoProviderFactory { CacheSignatureProviders = false }
                };

                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = (ClaimsIdentity)user.Identity,
                    Expires = DateTime.UtcNow.AddMinutes(30),
                    Issuer = issuer,
                    Audience = audience,
                    SigningCredentials = signingCredentials
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var stringToken = tokenHandler.WriteToken(token);

                return stringToken;
            }

        }

        #endregion
    }
}
