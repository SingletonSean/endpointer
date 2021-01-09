using Endpointer.Core.Client.Stores;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using System;

namespace Endpointer.Core.Client.Tests.Stores
{
    [TestFixture]
    public class AutoRefreshTokenStoreBaseTests
    {
        private AutoRefreshTokenStoreBase _store;

        private Mock<AutoRefreshTokenStoreBase> _mockStore;

        [SetUp]
        public void SetUp()
        {
            _mockStore = new Mock<AutoRefreshTokenStoreBase>();
            _mockStore.CallBase = true;

            _store = _mockStore.Object;
        }

        [Test]
        public void HasAccessToken_WithAccessToken_ReturnsTrue()
        {
            _mockStore.SetupGet(s => s.AccessToken).Returns("123test123");

            bool hasAccessToken = _store.HasAccessToken;

            Assert.IsTrue(hasAccessToken);
        }

        [Test]
        public void HasAccessToken_WithNullAccessToken_ReturnsFalse()
        {
            _mockStore.SetupGet(s => s.AccessToken).Returns(() => null);

            bool hasAccessToken = _store.HasAccessToken;

            Assert.IsFalse(hasAccessToken);
        }

        [Test]
        public void HasAccessToken_WithEmptyAccessToken_ReturnsFalse()
        {
            _mockStore.SetupGet(s => s.AccessToken).Returns(string.Empty);

            bool hasAccessToken = _store.HasAccessToken;

            Assert.IsFalse(hasAccessToken);
        }

        [Test]
        public void IsAccessTokenExpired_WithPastAccessTokenExpirationTime_ReturnsTrue()
        {
            _mockStore.Protected().SetupGet<DateTime>("AccessTokenExpirationTime").Returns(DateTime.UtcNow.AddDays(-1));

            bool expired = _store.IsAccessTokenExpired;

            Assert.IsTrue(expired);
        }

        [Test]
        public void IsAccessTokenExpired_WithCurrentTokenExpirationTime_ReturnsTrue()
        {
            _mockStore.Protected().SetupGet<DateTime>("AccessTokenExpirationTime").Returns(DateTime.UtcNow);

            bool expired = _store.IsAccessTokenExpired;

            Assert.IsTrue(expired);
        }

        [Test]
        public void IsAccessTokenExpired_WithFutureAccessTokenExpirationTime_ReturnsFalse()
        {
            _mockStore.Protected().SetupGet<DateTime>("AccessTokenExpirationTime").Returns(DateTime.UtcNow.AddDays(1));

            bool expired = _store.IsAccessTokenExpired;

            Assert.IsFalse(expired);
        }
    }
}
