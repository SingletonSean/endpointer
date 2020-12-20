using Endpointer.Authentication.API.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Endpointer.Authentication.API.Services.TokenDecoders
{
    public class AccessTokenDecoder
    {
        private readonly TokenValidationParameters _validationParameters;

        public AccessTokenDecoder(TokenValidationParameters validationParameters)
        {
            _validationParameters = validationParameters;
        }

        public Task<User> GetUserFromToken(string token)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            ClaimsPrincipal claims = handler.ValidateToken(token, _validationParameters, out SecurityToken securityToken);

            string rawId = claims.FindFirstValue("id");
            if(!Guid.TryParse(rawId, out Guid id))
            {
                throw new Exception("Unable to parse ID from JWT.");
            }

            string email = claims.FindFirstValue(ClaimTypes.Email);
            string username = claims.FindFirstValue(ClaimTypes.Name);

            return Task.FromResult(new User()
            {
                Id = id,
                Email = email,
                Username = username
            });
        }
    }
}
