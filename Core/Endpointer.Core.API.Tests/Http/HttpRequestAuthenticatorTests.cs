using System.Threading.Tasks;
using Endpointer.Core.API.Http;
using Endpointer.Core.API.Services.TokenDecoders;
using Moq;
using NUnit.Framework;

namespace Endpointer.Core.API.Tests.Http
{
    [TestFixture]
    public class HttpRequestAuthenticatorTests
    {
        private HttpRequestAuthenticator _authenticator;

        private Mock<IAccessTokenDecoder> _mockAccessTokenDecoder;

        [SetUp]
        public void SetUp()
        {
            _mockAccessTokenDecoder = new Mock<IAccessTokenDecoder>();

            _authenticator = new HttpRequestAuthenticator(_mockAccessTokenDecoder.Object);
        }

        [Test]
        public async Task Authenticate_WithEmptyAuthorizationHeader_ThrowsBearerSchemeNotProvidedException()
        {

        }

        [Test]
        public async Task Authenticate_WithMissingBearerScheme_ThrowsBearerSchemeNotProvidedException()
        {

        }

        [Test]
        public async Task Authenticate_WithSecurityTokenFailure_ThrowsSecurityTokenException()
        {

        }

        [Test]
        public async Task Authenticate_WithSecurityTokenDecryptionFailure_ThrowsSecurityTokenDecryptionFailedException()
        {

        }

        [Test]
        public async Task Authenticate_WithValidToken_ReturnsUser()
        {

        }
    }
}