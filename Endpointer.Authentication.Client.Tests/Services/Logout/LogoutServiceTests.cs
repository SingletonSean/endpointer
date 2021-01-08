using Endpointer.Authentication.Client.Services.Logout;
using Endpointer.Client.Tests.Services;
using Moq;
using NUnit.Framework;
using Refit;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Endpointer.Authentication.Client.Tests.Services.Logout
{
    [TestFixture]
    public class LogoutServiceTests : APIServiceTests
    {
        private LogoutService _service;

        private Mock<IAPILogoutService> _mockApi;

        private string _token;

        [SetUp]
        public void SetUp()
        {
            _mockApi = new Mock<IAPILogoutService>();

            _service = new LogoutService(_mockApi.Object);

            _token = "123test123";
        }

        [Test]
        public async Task Logout_WithUnknownApiException_ThrowsApiException()
        {
            ApiException apiException = await CreateApiException(HttpStatusCode.InternalServerError);
            _mockApi.Setup(s => s.Logout(_token)).ThrowsAsync(apiException);

            Assert.ThrowsAsync<ApiException>(() => _service.Logout(_token));
        }

        [Test]
        public void LogoutEverywhere_WithSuccess_DoesNotThrow()
        {
            Assert.DoesNotThrowAsync(() => _service.Logout(_token));
        }
    }
}
