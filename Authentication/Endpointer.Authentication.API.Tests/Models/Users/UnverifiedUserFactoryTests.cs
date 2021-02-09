using NUnit.Framework;
using Endpointer.Authentication.API.Models.Users;
using System;
using System.Collections.Generic;
using System.Text;
using Endpointer.Core.API.Models;

namespace Endpointer.Authentication.API.Tests.Models.Users
{
    [TestFixture()]
    public class UserFactoryTests
    {
        private UserFactory _factory;

        [SetUp]
        public void SetUp()
        {
            _factory = new UserFactory();
        }

        [Test()]
        public void CreateUser_WithValues_ReturnsVerifiedUser()
        {
            string email = "test@gmail.com";
            string username = "test";
            string passwordHash = "123test123";

            User user = _factory.CreateUser(email, username, passwordHash);

            Assert.AreEqual(email, user.Email);
            Assert.AreEqual(username, user.Username);
            Assert.AreEqual(passwordHash, user.PasswordHash);
            Assert.IsTrue(user.IsEmailVerified);
        }
    }
}