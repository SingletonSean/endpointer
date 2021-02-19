using NUnit.Framework;
using Endpointer.Authentication.API.Services.TokenValidators.EmailVerifications;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Endpointer.Authentication.API.Models;
using Endpointer.Core.API.Services.TokenClaimsDecoders;
using Moq;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace Endpointer.Authentication.API.Tests.Services.TokenValidators.EmailVerifications
{
    [TestFixture()]
    public class EmailVerificationTokenValidatorTests
    {
        private EmailVerificationTokenValidator _validator;

        private Mock<ITokenClaimsDecoder> _mockTokenDecoder;

        private string _token;
        private Guid _userId;
        private string _email;

        [SetUp]
        public void SetUp()
        {
            _mockTokenDecoder = new Mock<ITokenClaimsDecoder>();

            _validator = new EmailVerificationTokenValidator(
                _mockTokenDecoder.Object, 
                new EmailVerificationConfiguration()
                {
                    TokenSecret = "123123123123123123"
                },
                new Mock<ILogger<EmailVerificationTokenValidator>>().Object);

            _token = "test123123";
            _userId = Guid.NewGuid();
            _email = "test@gmail.com";
        }

        [Test()]
        public void Validate_WithValidTokenAndValidClaims_ReturnsCorrectTokenData()
        {
            _mockTokenDecoder
                .Setup(s => s.GetClaims(_token, It.IsAny<TokenValidationParameters>()))
                .Returns(CreateValidClaimsPrincipal());

            EmailVerificationToken tokenData = _validator.Validate(_token);

            Assert.AreEqual(_userId, tokenData.UserId);
            Assert.AreEqual(_email, tokenData.Email);
        }

        [Test()]
        public void Validate_WithValidTokenAndMissingUserIdClaim_ThrowsSecurityTokenDecryptionFailedException()
        {
            _mockTokenDecoder
                .Setup(s => s.GetClaims(_token, It.IsAny<TokenValidationParameters>()))
                .Returns(CreateMissingUserIdClaimsPrincipal());

            Assert.Throws<SecurityTokenDecryptionFailedException>(() => _validator.Validate(_token));
        }

        [Test()]
        public void Validate_WithValidTokenAndMissingEmailClaim_ThrowsSecurityTokenDecryptionFailedException()
        {
            _mockTokenDecoder
                .Setup(s => s.GetClaims(_token, It.IsAny<TokenValidationParameters>()))
                .Returns(CreateMissingEmailClaimsPrincipal());

            Assert.Throws<SecurityTokenDecryptionFailedException>(() => _validator.Validate(_token));
        }

        [Test()]
        public void Validate_WithInvalidToken_ThrowsSecurityTokenException()
        {
            _mockTokenDecoder.Setup(s => s.GetClaims(_token, It.IsAny<TokenValidationParameters>())).Throws(new SecurityTokenException());

            Assert.Throws<SecurityTokenException>(() => _validator.Validate(_token));
        }

        private ClaimsPrincipal CreateMissingUserIdClaimsPrincipal()
        {
            return new ClaimsPrincipal(new List<ClaimsIdentity>
            {
                new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Email, _email)
                })
            });
        }

        private ClaimsPrincipal CreateMissingEmailClaimsPrincipal()
        {
            return new ClaimsPrincipal(new List<ClaimsIdentity>
            {
                new ClaimsIdentity(new List<Claim>
                {
                    new Claim("id", _userId.ToString())
                })
            });
        }

        private ClaimsPrincipal CreateValidClaimsPrincipal()
        {
            return new ClaimsPrincipal(new List<ClaimsIdentity>
            {
                new ClaimsIdentity(new List<Claim>
                {
                    new Claim("id", _userId.ToString()),
                    new Claim(ClaimTypes.Email, _email)
                })
            });
        }
    }
}