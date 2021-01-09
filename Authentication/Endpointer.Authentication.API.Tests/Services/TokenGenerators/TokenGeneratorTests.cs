using Endpointer.Authentication.API.Services.TokenGenerators;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Endpointer.Authentication.API.Tests.Services.TokenGenerators
{
    [TestFixture]
    public class TokenGeneratorTests
    {
        private TokenGenerator _tokenGenerator;

        [SetUp]
        public void Setup()
        {
            _tokenGenerator = new TokenGenerator();
        }

        [Test]
        public void GenerateToken_WithFailure_ThrowsException()
        {
            Assert.Throws<Exception>(() => _tokenGenerator.GenerateToken(null, null, null, DateTime.Now, null));
        }

        [Test]
        public void GenerateToken_WithSuccess_ReturnsToken()
        {
            string token = _tokenGenerator.GenerateToken(
                "3y7XS2AHicSOs2uUJCxwlHWqTJNExW3UDUjMeXi96uLEso1YV4RazqQubpFBdx0zZGtdxBelKURhh0WXxPR0mEJQHk_0U9HeYtqcMManhoP3X2Ge8jgxh6k4C_Gd4UPTc6lkx0Ca5eRE16ciFQ6wmYDnaXC8NbngGqartHccAxE", 
                null, null, DateTime.Now.AddDays(3), null);

            Assert.IsFalse(string.IsNullOrEmpty(token));
        }
    }
}
