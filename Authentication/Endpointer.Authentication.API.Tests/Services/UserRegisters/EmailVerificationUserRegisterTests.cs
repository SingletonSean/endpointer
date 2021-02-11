using NUnit.Framework;
using Endpointer.Authentication.API.Services.UserRegisters;
using System;
using System.Collections.Generic;
using System.Text;
using Endpointer.Core.API.Models;
using Moq;
using Endpointer.Authentication.API.Services.UserRepositories;
using System.Threading.Tasks;
using Endpointer.Authentication.API.Services.EmailSenders;
using Endpointer.Authentication.API.Models;
using Endpointer.Authentication.API.Services.TokenGenerators.EmailVerifications;
using Endpointer.Authentication.API.Exceptions;
using Microsoft.Extensions.Logging;
using Endpointer.Authentication.API.Services.EmailVerificationSenders;

namespace Endpointer.Authentication.API.Tests.Services.UserRegisters
{
    [TestFixture()]
    public class EmailVerificationUserRegisterTests
    {
        private EmailVerificationUserRegister _userRegister;

        private Mock<IUserRepository> _mockUserRepository;
        private Mock<IEmailVerificationSender> _mockEmailSender;

        private string _email;
        private string _username;
        private string _passwordHash;

        [SetUp]
        public void SetUp()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockEmailSender = new Mock<IEmailVerificationSender>();

            _userRegister = new EmailVerificationUserRegister(
                _mockUserRepository.Object, 
                _mockEmailSender.Object,
                new Mock<ILogger<EmailVerificationUserRegister>>().Object);

            _email = "test@gmail.com";
            _username = "test";
            _passwordHash = "123test123";
        }

        [Test()]
        public async Task RegisterUser_WithSuccess_ReturnsUserWithCorrectData()
        {
            User user = await _userRegister.RegisterUser(_email, _username, _passwordHash);

            Assert.IsNotNull(user);
            Assert.AreEqual(_email, user.Email);
            Assert.AreEqual(_username, user.Username);
            Assert.AreEqual(_passwordHash, user.PasswordHash);
            Assert.IsFalse(user.IsEmailVerified);
        }

        [Test()]
        public async Task RegisterUser_WithSuccess_CallsRepositoryWithNonEmailVerifiedAndNoIdUser()
        {
            await _userRegister.RegisterUser(_email, _username, _passwordHash);

            _mockUserRepository.Verify(r => r.Create(ExpectedNonEmailVerifiedUser()), Times.Once);
        }

        [Test()]
        public async Task RegisterUser_WithSuccess_SendsEmailVerificationEmail()
        {
            await _userRegister.RegisterUser(_email, _username, _passwordHash);

            _mockEmailSender.Verify(s => s.SendEmailVerificationEmail(ExpectedNonEmailVerifiedUser()), Times.Once);
        }

        [Test()]
        public void RegisterUser_WithRepositoryException_ThrowsException()
        {
            _mockUserRepository.Setup(s => s.Create(ExpectedNonEmailVerifiedUser())).ThrowsAsync(new Exception());

            Assert.ThrowsAsync<Exception>(() => _userRegister.RegisterUser(_email, _username, _passwordHash));
        }

        [Test()]
        public void RegisterUser_WithEmailSenderException_ThrowsSendEmailException()
        {
            _mockEmailSender.Setup(s => s.SendEmailVerificationEmail(ExpectedNonEmailVerifiedUser())).Throws(new Exception());

            SendEmailException exception = Assert.ThrowsAsync<SendEmailException>(() => _userRegister.RegisterUser(_email, _username, _passwordHash));

            Assert.AreEqual(_email, exception.To);
        }

        private User ExpectedNonEmailVerifiedUser()
        {
            return It.Is<User>(u => u.Email == _email &&
                u.Username == _username &&
                u.PasswordHash == _passwordHash &&
                u.IsEmailVerified == false);
        }
    }
}