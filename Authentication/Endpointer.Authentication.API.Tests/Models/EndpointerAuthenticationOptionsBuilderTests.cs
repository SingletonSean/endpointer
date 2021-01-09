using NUnit.Framework;
using Endpointer.Authentication.API.Models;

namespace Endpointer.Authentication.API.Tests.Models
{
    [TestFixture()]
    public class EndpointerAuthenticationOptionsBuilderTests
    {
        private EndpointerAuthenticationOptionsBuilder _builder;

        [SetUp]
        public void Setup()
        {
            _builder = new EndpointerAuthenticationOptionsBuilder();
        }

        [Test()]
        public void Build_WithDatabase_ReturnsOptionsWithUseDatabaseTrue()
        {
            EndpointerAuthenticationOptions options = _builder.WithDatabase().Build();

            Assert.IsTrue(options.UseDatabase);
        }

        [Test()]
        public void Build_WithDatabase_ReturnsOptionsWithDatabaseServices()
        {
            EndpointerAuthenticationOptions options = _builder.WithDatabase().Build();

            Assert.IsNotNull(options.AddDbContext);
            Assert.IsNotNull(options.AddDbRefreshTokenRepository);
            Assert.IsNotNull(options.AddDbUserRepository);
        }

        [Test()]
        public void Build_WithoutConfiguration_ReturnsOptionsWithUseDatabaseFalse()
        {
            EndpointerAuthenticationOptions options = _builder.Build();

            Assert.IsFalse(options.UseDatabase);
        }

        [Test()]
        public void Build_WithoutConfiguration_ReturnsOptionsWithoutDatabaseServices()
        {
            EndpointerAuthenticationOptions options = _builder.Build();

            Assert.IsNull(options.AddDbContext);
            Assert.IsNull(options.AddDbRefreshTokenRepository);
            Assert.IsNull(options.AddDbUserRepository);
        }
    }
}