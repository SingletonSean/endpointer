using Endpointer.Core.Client.Models;
using Endpointer.Core.Client.Stores;
using Moq;
using NUnit.Framework;
using Refit;

namespace Endpointer.Core.Client.Tests.Models
{
    [TestFixture]
    public class EndpointerClientOptionsBuilderTests
    {
        private EndpointerClientOptionsBuilder _builder;

        [SetUp]
        public void SetUp()
        {
            _builder = new EndpointerClientOptionsBuilder();
        }

        [Test]
        public void UseRefitSettings_WithSettings_UsesSettingsOnBuild()
        {
            RefitSettings expected = new RefitSettings();

            EndpointerClientOptions options = _builder.UseRefitSettings(expected).Build();

            Assert.AreEqual(expected, options.RefitSettings);
        }

        [Test]
        public void WithAutoTokenRefresh_WithAutoTokenRefreshInstance_EnablesAutoTokenRefreshOnBuild()
        {
            EndpointerClientOptions options = _builder.WithAutoTokenRefresh(It.IsAny<IAutoRefreshTokenStore>()).Build();

            Assert.IsTrue(options.AutoTokenRefresh);
        }

        [Test]
        public void WithAutoTokenRefresh_WithAutoTokenRefreshInstance_SetsGetAutoTokenRefreshFuncOnBuild()
        {
            IAutoRefreshTokenStore expected = new Mock<IAutoRefreshTokenStore>().Object;

            EndpointerClientOptions options = _builder.WithAutoTokenRefresh(expected).Build();

            IAutoRefreshTokenStore actual = options.GetAutoRefreshTokenStore(null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void WithAutoTokenRefresh_WithAutoTokenRefreshFunc_EnablesAutoTokenRefreshOnBuild()
        {
            EndpointerClientOptions options = _builder.WithAutoTokenRefresh((s) => It.IsAny<IAutoRefreshTokenStore>()).Build();

            Assert.IsTrue(options.AutoTokenRefresh);
        }

        [Test]
        public void WithAutoTokenRefresh_WithAutoTokenRefreshFunc_SetsGetAutoTokenRefreshFuncOnBuild()
        {
            IAutoRefreshTokenStore expected = new Mock<IAutoRefreshTokenStore>().Object;

            EndpointerClientOptions options = _builder.WithAutoTokenRefresh((s) => expected).Build();

            IAutoRefreshTokenStore actual = options.GetAutoRefreshTokenStore(null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Build_WithNoConfiguration_ReturnsOptionsWithDefaultRefitSettings()
        {
            EndpointerClientOptions options = _builder.Build();

            Assert.IsNotNull(options.RefitSettings);
        }

        [Test]
        public void Build_WithNoConfiguration_ReturnsOptionsWithFalseAutoTokenRefresh()
        {
            EndpointerClientOptions options = _builder.Build();

            Assert.IsFalse(options.AutoTokenRefresh);
        }

        [Test]
        public void Build_WithNoConfiguration_ReturnsOptionsWithNullGetAutoRefreshTokenStore()
        {
            EndpointerClientOptions options = _builder.Build();

            Assert.IsNull(options.GetAutoRefreshTokenStore);
        }
    }
}
