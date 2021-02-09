using Endpointer.Core.API.Models;
using Endpointer.Core.API.Services.TokenClaimsDecoders;
using Endpointer.Core.API.Services.TokenDecoders;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Moq;
using NUnit.Framework;
using System;
using System.Security.Claims;
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

            _decoder = new AccessTokenDecoder(_mockTokenClaimsDecoder.Object, new Mock<ILogger<AccessTokenDecoder>>().Object);
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
        public async Task GetUserFromToken_WithNoEmailVerifiedClaim_ReturnsUserWithIsEmailVerifiedFalse()
        {
            ClaimsPrincipal claims = CreateClaims(
                new Claim("id", Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Email, "test@gmail.com"),
                new Claim(ClaimTypes.Name, "test"));
            _mockTokenClaimsDecoder.Setup(d => d.GetClaims(_token)).Returns(claims);

            User user = await _decoder.GetUserFromToken(_token);

            Assert.IsFalse(user.IsEmailVerified);
        }

        [Test]
        public async Task GetUserFromToken_WithEmptyEmailVerifiedClaim_ReturnsUserWithIsEmailVerifiedFalse()
        {
            ClaimsPrincipal claims = CreateClaims(
                new Claim("id", Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Email, "test@gmail.com"),
                new Claim(ClaimTypes.Name, "test"),
                new Claim(ClaimKey.EMAIL_VERIFIED, string.Empty));
            _mockTokenClaimsDecoder.Setup(d => d.GetClaims(_token)).Returns(claims);

            User user = await _decoder.GetUserFromToken(_token);

            Assert.IsFalse(user.IsEmailVerified);
        }

        [Test]
        public async Task GetUserFromToken_WithTrueEmailVerifiedClaim_ReturnsUserWithIsEmailVerifiedTrue()
        {
            ClaimsPrincipal claims = CreateClaims(
                new Claim("id", Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Email, "test@gmail.com"),
                new Claim(ClaimTypes.Name, "test"),
                new Claim(ClaimKey.EMAIL_VERIFIED, "true"));
            _mockTokenClaimsDecoder.Setup(d => d.GetClaims(_token)).Returns(claims);

            User user = await _decoder.GetUserFromToken(_token);

            Assert.IsTrue(user.IsEmailVerified);
        }

        [Test]
        public async Task GetUserFromToken_WithFalseEmailVerifiedClaim_ReturnsUserWithIsEmailVerifiedFalse()
        {
            ClaimsPrincipal claims = CreateClaims(
                new Claim("id", Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Email, "test@gmail.com"),
                new Claim(ClaimTypes.Name, "test"),
                new Claim(ClaimKey.EMAIL_VERIFIED, "false"));
            _mockTokenClaimsDecoder.Setup(d => d.GetClaims(_token)).Returns(claims);

            User user = await _decoder.GetUserFromToken(_token);

            Assert.IsFalse(user.IsEmailVerified);
        }

        [Test]
        public async Task GetUserFromToken_WithValidToken_ReturnsDecodedUserAsync()
        {
            Guid userId = Guid.NewGuid();
            string email = "test@gmail.com";
            string username = "test";
            bool emailVerified = true;
            ClaimsPrincipal claims = CreateClaims(
                new Claim("id", userId.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimKey.EMAIL_VERIFIED, emailVerified.ToString()));
            _mockTokenClaimsDecoder.Setup(d => d.GetClaims(_token)).Returns(claims);

            User user = await _decoder.GetUserFromToken(_token);

            Assert.IsNotNull(user);
            Assert.AreEqual(userId, user.Id);
            Assert.AreEqual(email, user.Email);
            Assert.AreEqual(username, user.Username);
            Assert.AreEqual(emailVerified, user.IsEmailVerified);
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
