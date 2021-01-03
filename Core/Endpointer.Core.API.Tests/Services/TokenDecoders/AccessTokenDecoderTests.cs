using Endpointer.Core.API.Models;
using Endpointer.Core.API.Services.TokenClaimsDecoders;
using Endpointer.Core.API.Services.TokenDecoders;
using Microsoft.IdentityModel.Tokens;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Endpointer.Core.API.Tests.Services.TokenDecoders
{
    [TestFixture]
    public class AccessTokenDecoderTests
    {
        private AccessTokenDecoder _decoder;

        private Mock<ITokenClaimsDecoder> _mockTokenClaimsDecoder;

        private string _token = "123test123";

        [SetUp]
        public void SetUp()
        {
            _mockTokenClaimsDecoder = new Mock<ITokenClaimsDecoder>();

            _decoder = new AccessTokenDecoder(_mockTokenClaimsDecoder.Object);
        }

        [Test]
        public void GetUserFromToken_WithSecurityTokenException_ThrowsSecurityTokenException()
        {
            _mockTokenClaimsDecoder.Setup(d => d.GetClaims(_token)).Throws(new SecurityTokenException());

            Assert.ThrowsAsync<SecurityTokenException>(() => _decoder.GetUserFromToken(_token));
        }

        [Test]
        public void GetUserFromToken_WithNoIdClaim_ThrowsSecurityTokenDecryptionFailedException()
        {
            ClaimsPrincipal claims = new ClaimsPrincipal();
            _mockTokenClaimsDecoder.Setup(d => d.GetClaims(_token)).Returns(claims);

            Assert.ThrowsAsync<SecurityTokenDecryptionFailedException>(() => _decoder.GetUserFromToken(_token));
        }

        [Test]
        public void GetUserFromToken_WithNonGuidIdClaim_ThrowsSecurityTokenDecryptionFailedException()
        {
            ClaimsPrincipal claims = CreateClaims(new Claim("id", "not_a_guid"));
            _mockTokenClaimsDecoder.Setup(d => d.GetClaims(_token)).Returns(claims);

            Assert.ThrowsAsync<SecurityTokenDecryptionFailedException>(() => _decoder.GetUserFromToken(_token));
        }

        [Test]
        public void GetUserFromToken_WithNoEmailClaim_ThrowsSecurityTokenDecryptionFailedException()
        {
            ClaimsPrincipal claims = CreateClaims(new Claim("id", Guid.NewGuid().ToString()));
            _mockTokenClaimsDecoder.Setup(d => d.GetClaims(_token)).Returns(claims);

            Assert.ThrowsAsync<SecurityTokenDecryptionFailedException>(() => _decoder.GetUserFromToken(_token));
        }

        [Test]
        public void GetUserFromToken_WithNoNameClaim_ThrowsSecurityTokenDecryptionFailedException()
        {
            ClaimsPrincipal claims = CreateClaims(
                new Claim("id", Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Email, "test@gmail.com"));
            _mockTokenClaimsDecoder.Setup(d => d.GetClaims(_token)).Returns(claims);

            Assert.ThrowsAsync<SecurityTokenDecryptionFailedException>(() => _decoder.GetUserFromToken(_token));
        }

        [Test]
        public async Task GetUserFromToken_WithValidToken_ReturnsDecodedUserAsync()
        {
            Guid userId = Guid.NewGuid();
            string email = "test@gmail.com";
            string username = "test";
            ClaimsPrincipal claims = CreateClaims(
                new Claim("id", userId.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Name, username));
            _mockTokenClaimsDecoder.Setup(d => d.GetClaims(_token)).Returns(claims);

            User user = await _decoder.GetUserFromToken(_token);

            Assert.IsNotNull(user);
            Assert.AreEqual(userId, user.Id);
            Assert.AreEqual(email, user.Email);
            Assert.AreEqual(username, user.Username);
        }

        private ClaimsPrincipal CreateClaims(params Claim[] claims)
        {
            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            
            foreach (Claim claim in claims)
            {
                claimsIdentity.AddClaim(claim);
            }

            return new ClaimsPrincipal(new ClaimsIdentity[] { claimsIdentity });
        }
    }
}
