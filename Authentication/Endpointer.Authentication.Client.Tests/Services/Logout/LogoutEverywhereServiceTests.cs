using Endpointer.Authentication.Client.Services.Logout;
using Endpointer.Client.Tests.Services;
using Endpointer.Core.Client.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Refit;
using System.Net;
using System.Threading.Tasks;

namespace Endpointer.Authentication.Client.Tests.Services.Logout
{
    [TestFixture]
    public class LogoutEverywhereServiceTests : APIServiceTests
    {
        private LogoutEverywhereService _service;

        private Mock<IAPILogoutEverywhereService> _mockApi;

        [SetUp]
        public void SetUp()
        {
            _mockApi = new Mock<IAPILogoutEverywhereService>();

            _service = new LogoutEverywhereService(_mockApi.Object, new Mock<ILogger<LogoutEverywhereService>>().Object);
        }

        [Test]
        public async Task LogoutEverywhere_WithUnauthorizedApiException_ThrowsUnauthorizedException()
        {
            ApiException apiException = await CreateApiException(HttpStatusCode.Unauthorized);
            _mockApi.Setup(s => s.LogoutEverywhere()).ThrowsAsync(apiException);

            Assert.ThrowsAsync<UnauthorizedException>(() => _service.LogoutEverywhere());
        }

        [Test]
        public async Task LogoutEverywhere_WithUnknownApiException_ThrowsApiException()
        {
            ApiException apiException = await CreateApiException(HttpStatusCode.InternalServerError);
            _mockApi.Setup(s => s.LogoutEverywhere()).ThrowsAsync(apiException);

            Assert.ThrowsAsync<ApiException>(() => _service.LogoutEverywhere());
        }

        [Test]
        public void LogoutEverywhere_WithSuccess_DoesNotThrow()
        {
            Assert.DoesNotThrowAsync(() => _service.LogoutEverywhere());
        }
    }
}
