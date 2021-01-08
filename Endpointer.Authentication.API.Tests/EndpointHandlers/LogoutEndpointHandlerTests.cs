using Endpointer.Authentication.API.EndpointHandlers;
using Endpointer.Authentication.API.Services.RefreshTokenRepositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Endpointer.Authentication.API.Tests.EndpointHandlers
{
    [TestFixture]
    public class LogoutEndpointHandlerTests
    {
        private LogoutEndpointHandler _handler;
        
        private Mock<IRefreshTokenRepository> _mockRefreshTokenRepository;

        private string _refreshToken;

        [SetUp]
        public void SetUp()
        {
            _mockRefreshTokenRepository = new Mock<IRefreshTokenRepository>();

            _handler = new LogoutEndpointHandler(_mockRefreshTokenRepository.Object);

            _refreshToken = "123test123";
        }

        [Test]
        public void HandleLogout_WithException_ThrowsException()
        {
            _mockRefreshTokenRepository.Setup(s => s.DeleteByToken(_refreshToken)).ThrowsAsync(new Exception());

            Assert.ThrowsAsync<Exception>(() => _handler.HandleLogout(_refreshToken));
        }

        [Test]
        public async Task HandleLogout_WithSuccess_ReturnsNoContentResult()
        {
            IActionResult result = await _handler.HandleLogout(_refreshToken);

            Assert.IsAssignableFrom<NoContentResult>(result);
        }
    }
}
