using Endpointer.Core.API.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Endpointer.Core.API.Services.TokenDecoders
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
            ClaimsPrincipal claims = handler.ValidateToken(token, _validationParameters, out _);
            
            string rawId = claims.FindFirst("id")?.Value;
            if(!Guid.TryParse(rawId, out Guid id))
            {
                throw new Exception("Unable to parse ID from JWT.");
            }

            string email = claims.FindFirst(ClaimTypes.Email)?.Value;
            string username = claims.FindFirst(ClaimTypes.Name)?.Value;

            return Task.FromResult(new User()
            {
                Id = id,
                Email = email,
                Username = username
            });
        }
    }
}
