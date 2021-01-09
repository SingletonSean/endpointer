using NUnit.Framework;
using Endpointer.Accounts.Client.Services.Accounts;
using System;
using System.Collections.Generic;
using System.Text;
using Endpointer.Client.Tests.Services;
using Moq;
using Endpointer.Core.Models.Responses;
using Endpointer.Accounts.Core.Models.Responses;
using System.Threading.Tasks;
using Refit;
using System.Net;
using Endpointer.Core.Client.Exceptions;
using Endpointer.Accounts.Client.Exceptions;

namespace Endpointer.Accounts.Clients.Tests.Services.Accounts
{
    [TestFixture()]
    public class AccountServiceTests : APIServiceTests
    {
        private AccountService _service;

        private Mock<IAPIAccountService> _mockApi;

        [SetUp]
        public void Setup()
        {
            _mockApi = new Mock<IAPIAccountService>();

            _service = new AccountService(_mockApi.Object);
        }

        [Test]
        public void GetAccount_WithNullResponse_ThrowsException()
        {
            _mockApi.Setup(s => s.GetAccount()).ReturnsAsync(() => null);

            Assert.ThrowsAsync<Exception>(() => _service.GetAccount());
        }

        [Test]
        public void GetAccount_WithNullResponseData_ThrowsException()
        {
            _mockApi.Setup(s => s.GetAccount()).ReturnsAsync(() => new SuccessResponse<AccountResponse>(null));

            Assert.ThrowsAsync<Exception>(() => _service.GetAccount());
        }

        [Test]
        public async Task GetAccount_WithUnauthorizedApiException_ThrowsUnauthorizedException()
        {
            ApiException exception = await CreateApiException(HttpStatusCode.Unauthorized);
            _mockApi.Setup(s => s.GetAccount()).ThrowsAsync(exception);

            Assert.ThrowsAsync<UnauthorizedException>(() => _service.GetAccount());
        }

        [Test]
        public async Task GetAccount_WithNotFoundApiException_ThrowsAccountNotFoundException()
        {
            ApiException exception = await CreateApiException(HttpStatusCode.NotFound);
            _mockApi.Setup(s => s.GetAccount()).ThrowsAsync(exception);

            Assert.ThrowsAsync<AccountNotFoundException>(() => _service.GetAccount());
        }

        [Test]
        public async Task GetAccount_WithUnknownApiException_ThrowsApiException()
        {
            ApiException exception = await CreateApiException(HttpStatusCode.BadRequest);
            _mockApi.Setup(s => s.GetAccount()).ThrowsAsync(exception);

            Assert.ThrowsAsync<ApiException>(() => _service.GetAccount());
        }

        [Test]
        public async Task GetAccount_WithSuccess_ReturnsAccount()
        {
            _mockApi.Setup(s => s.GetAccount()).ReturnsAsync(() => new SuccessResponse<AccountResponse>(new AccountResponse()));

            AccountResponse response = await _service.GetAccount();

            Assert.IsNotNull(response);
        }
    }
}