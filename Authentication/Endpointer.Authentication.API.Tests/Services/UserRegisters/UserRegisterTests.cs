using NUnit.Framework;
using Endpointer.Authentication.API.Services.UserRegisters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Endpointer.Core.API.Models;
using Moq;
using Endpointer.Authentication.API.Services.UserRepositories;

namespace Endpointer.Authentication.API.Tests.Services.UserRegisters
{
    [TestFixture()]
    public class UserRegisterTests
    {
        private UserRegister _userRegister;

        private Mock<IUserRepository> _mockUserRepository;

        private string _email;
        private string _username;
        private string _passwordHash;

        [SetUp]
        public void SetUp()
        {
            _mockUserRepository = new Mock<IUserRepository>();

            _userRegister = new UserRegister(_mockUserRepository.Object);

            _email = "test@gmail.com";
            _username = "test";
            _passwordHash = "123test123";
        }

        [Test()]
        public async Task RegisterUser_WithSuccess_ReturnsUser()
        {
            User expectedUser = new User();
            _mockUserRepository.Setup(s => s.Create(ExpectedEmailVerifiedUser())).ReturnsAsync(expectedUser);

            User user = await _userRegister.RegisterUser(_email, _username, _passwordHash);

            Assert.IsNotNull(user);
            Assert.AreEqual(expectedUser, user);
        }

        [Test()]
        public async Task RegisterUser_WithSuccess_CallsRepositoryWithEmailVerifiedAndNoIDUser()
        {
            _mockUserRepository.Setup(s => s.Create(ExpectedEmailVerifiedUser())).ReturnsAsync(new User());

            await _userRegister.RegisterUser(_email, _username, _passwordHash);

            _mockUserRepository.Verify(r => r.Create(ExpectedEmailVerifiedUser()), Times.Once);
        }

        [Test()]
        public void RegisterUser_WithExcpetion_ThrowsException()
        {
            _mockUserRepository.Setup(s => s.Create(ExpectedEmailVerifiedUser())).ThrowsAsync(new Exception());

            Assert.ThrowsAsync<Exception>(() => _userRegister.RegisterUser(_email, _username, _passwordHash));
        }

        private User ExpectedEmailVerifiedUser()
        {
            return It.Is<User>(u => u.Email == _email && 
                u.Username == _username && 
                u.PasswordHash == _passwordHash &&
                u.IsEmailVerified == true);
        }
    }
}