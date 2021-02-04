using NUnit.Framework;
using Endpointer.Authentication.API.Models;
using Microsoft.Extensions.DependencyInjection;
using Endpointer.Authentication.API.Services.UserRepositories;
using Endpointer.Authentication.API.Services.RefreshTokenRepositories;
using Moq;
using System;

namespace Endpointer.Authentication.API.Tests.Models
{
    [TestFixture()]
    public class EndpointerAuthenticationOptionsBuilderTests
    {
        private EndpointerAuthenticationOptionsBuilder _builder;

        private Mock<IServiceCollection> _mockServices;

        private IServiceCollection Services => _mockServices.Object;

        [SetUp]
        public void Setup()
        {
            _builder = new EndpointerAuthenticationOptionsBuilder();

            _mockServices = new Mock<IServiceCollection>();
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
            Assert.IsNotNull(options.AddRefreshTokenRepository);
            Assert.IsNotNull(options.AddUserRepository);
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
        }

        [Test()]
        public void Build_WithoutConfiguration_ReturnsOptionsWithInMemoryDataSourceServices()
        {
            EndpointerAuthenticationOptions options = _builder.Build();

            options.AddUserRepository(Services);
            options.AddRefreshTokenRepository(Services);
            VerifyServiceAdded(typeof(IUserRepository), typeof(InMemoryUserRepository));
            VerifyServiceAdded(typeof(IRefreshTokenRepository), typeof(InMemoryRefreshTokenRepository));
        }

        private void VerifyServiceAdded(Type inter, Type implem)
        {
            _mockServices.Verify(s => s.Add(It.Is<ServiceDescriptor>(s => s.ServiceType == inter && s.ImplementationType == implem)));
        }
    }
}