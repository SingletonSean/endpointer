using Endpointer.Authentication.API.Models;
using Endpointer.Authentication.API.Services.TokenValidators;
using Microsoft.IdentityModel.Tokens;
using NUnit.Framework;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Endpointer.Authentication.API.Tests.Services.TokenValidators
{
    [TestFixture]
    public class RefreshTokenValidatorTests
    {
        private RefreshTokenValidator _validator;

        private string _token;

        [SetUp]
        public void SetUp()
        {
            AuthenticationConfiguration configuration = new AuthenticationConfiguration()
            { 
                RefreshTokenSecret = "3y7XS2AHicSOs2uUJCxwlHWqTJNExW3UDUjMeXi96uLEso1YV4RazqQubpFBdx0zZGtdxBelKURhh0WXxPR0mEJQHk_0U9HeYtqcMManhoP3X2Ge8jgxh6k4C_Gd4UPTc6lkx0Ca5eRE16ciFQ6wmYDnaXC8NbngGqartHccAxE",
                Issuer = "test",
                Audience = "test"
            };

            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.RefreshTokenSecret));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken token = new JwtSecurityToken(configuration.Issuer, 
                configuration.Audience, null, DateTime.Now, DateTime.Now.AddDays(3), credentials);

            _token = new JwtSecurityTokenHandler().WriteToken(token);

            _validator = new RefreshTokenValidator(configuration);
        }

        [Test]
        public void Validate_WithValidToken_ReturnsTrue()
        {
            bool valid = _validator.Validate(_token);

            Assert.IsTrue(valid);
        }

        [Test]
        public void Validate_WithInvalidToken_ReturnsFalse()
        {
            bool valid = _validator.Validate(string.Empty);

            Assert.IsFalse(valid);
        }
    }
}
