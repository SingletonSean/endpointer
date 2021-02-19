using Endpointer.Authentication.API.Models;
using Endpointer.Authentication.API.Services.TokenValidators;
using Endpointer.Core.API.Services.TokenClaimsDecoders;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Moq;
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

        private Mock<ITokenClaimsDecoder> _mockTokenClaimsDecoder;

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
            _mockTokenClaimsDecoder = new Mock<ITokenClaimsDecoder>();

            _validator = new RefreshTokenValidator(_mockTokenClaimsDecoder.Object, 
                configuration, 
                new Mock<ILogger<RefreshTokenValidator>>().Object);
            
            _token = "123test123";
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
            _mockTokenClaimsDecoder.Setup(d => d.GetClaims(_token, It.IsAny<TokenValidationParameters>())).Throws(new SecurityTokenException());
            
            bool valid = _validator.Validate(_token);

            Assert.IsFalse(valid);
        }
    }
}
