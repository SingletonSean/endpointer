using NUnit.Framework;
using Endpointer.Authentication.API.Services.EmailVerificationSenders;
using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Endpointer.Authentication.API.Services.TokenGenerators.EmailVerifications;
using Endpointer.Authentication.API.Services.EmailSenders;
using Endpointer.Authentication.API.Models;
using Endpointer.Core.API.Models;

namespace Endpointer.Authentication.API.Tests.Services.EmailVerificationSenders
{
    [TestFixture()]
    public class EmailVerificationSenderTests
    {
        private EmailVerificationSender _sender;

        private Mock<IEmailVerificationTokenGenerator> _mockTokenGenerator;
        private Mock<IEmailSender> _mockEmailSender;
        private EmailVerificationConfiguration _configuration;

        private User _user;

        [SetUp]
        public void SetUp()
        {
            _mockTokenGenerator = new Mock<IEmailVerificationTokenGenerator>();
            _mockEmailSender = new Mock<IEmailSender>();
            _configuration = new EmailVerificationConfiguration()
            {
                VerifyBaseUrl = "localhost:5000/verify",
                CreateEmailSubject = (username) => $"test {username}"
            };

            _sender = new EmailVerificationSender(
                _mockEmailSender.Object,
                _mockTokenGenerator.Object,
                _configuration,
                new Mock<ILogger<EmailVerificationSender>>().Object);

            _user = new User()
            {
                Email = "test@gmail.com",
                Username = "test"
            };
        }

        [Test()]
        public void SendEmailVerificationEmail_WithEmailSendException_ThrowsException()
        {
            _mockEmailSender
                .Setup(s => s.Send(It.IsAny<string>(), It.IsAny<string>(), _user.Email, It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception());

            Assert.ThrowsAsync<Exception>(() => _sender.SendEmailVerificationEmail(_user));
        }

        [Test()]
        public void SendEmailVerificationEmail_WithTokenGenerationExceptions_ThrowsException()
        {
            _mockTokenGenerator
                .Setup(s => s.GenerateToken(_user))
                .Throws(new Exception());

            Assert.ThrowsAsync<Exception>(() => _sender.SendEmailVerificationEmail(_user));
        }

        [Test()]
        public async Task SendEmailVerificationEmail_WithSuccess_SendsEmailToUser()
        {
            await _sender.SendEmailVerificationEmail(_user);

            _mockEmailSender.Verify(
                s => s.Send(It.IsAny<string>(), It.IsAny<string>(), _user.Email, It.IsAny<string>(), It.IsAny<string>()),
                Times.Once);
        }

        [Test()]
        public async Task SendEmailVerificationEmail_WithSuccess_SendsEmailVerificationEmailWithCorrectSubject()
        {
            string expectedSubject = _configuration.CreateEmailSubject(_user.Username);

            await _sender.SendEmailVerificationEmail(_user);

            _mockEmailSender.Verify(
                s => s.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), expectedSubject, It.IsAny<string>()),
                Times.Once);
        }

        [Test()]
        public async Task SendEmailVerificationEmail_WithSuccess_SendsEmailVerificationEmailWithCorrectVerifyUrl()
        {
            string token = "123test123";
            string expectedUrl = $"{_configuration.VerifyBaseUrl}?token={token}";
            _mockTokenGenerator.Setup(s => s.GenerateToken(_user)).Returns(token);

            await _sender.SendEmailVerificationEmail(_user);

            _mockEmailSender.Verify(
                s => s.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), 
                    It.Is<string>(s => s.Contains(expectedUrl))),
                Times.Once);
        }
    }
}