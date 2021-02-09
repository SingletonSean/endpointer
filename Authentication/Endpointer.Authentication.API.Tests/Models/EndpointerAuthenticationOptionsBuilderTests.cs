using NUnit.Framework;
using Endpointer.Authentication.API.Models;
using Microsoft.Extensions.DependencyInjection;
using Endpointer.Authentication.API.Services.UserRepositories;
using Endpointer.Authentication.API.Services.RefreshTokenRepositories;
using Moq;
using System;
using Endpointer.Authentication.API.Contexts;
using System.Collections.Generic;

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
            _mockServices.Setup(s => s.GetEnumerator()).Returns(new List<ServiceDescriptor>().GetEnumerator());
        }

        [Test()]
        public void Build_RequireEmailVerification_ReturnsOptionsWithRequireEmailVerificationTrue()
        {
            EndpointerAuthenticationOptions options = _builder.RequireEmailVerification().Build();

            Assert.IsTrue(options.RequireEmailVerification);
        }

        [Test()]
        public void Build_WithEntityFrameworkDataSource_ReturnsOptionsWithEntityFrameworkDataSourceServices()
        {
            EndpointerAuthenticationOptions options = _builder.WithEntityFrameworkDataSource().Build();

            options.AddDataSourceServices(Services);
            VerifyServiceAdded(typeof(DefaultAuthenticationDbContext), It.IsAny<DefaultAuthenticationDbContext>());
            VerifyServiceAdded(typeof(IUserRepository), typeof(DatabaseUserRepository<DefaultAuthenticationDbContext>));
            VerifyServiceAdded(typeof(IRefreshTokenRepository), typeof(DatabaseRefreshTokenRepository<DefaultAuthenticationDbContext>));
        }

        [Test()]
        public void Build_WithCustomDataSource_ReturnsOptionsWithCustomDataSourceServices()
        {
            IUserRepository customUserRepository = new Mock<IUserRepository>().Object;
            IRefreshTokenRepository customRefreshRepository = new Mock<IRefreshTokenRepository>().Object;
            Action<IServiceCollection> addCustomDataSource = (s) => 
            {
                s.AddSingleton(customUserRepository);
                s.AddSingleton(customRefreshRepository);
            };

            EndpointerAuthenticationOptions options = _builder.WithCustomDataSource(addCustomDataSource).Build();

            options.AddDataSourceServices(Services);
            VerifyServiceAdded(typeof(IUserRepository), customUserRepository);
            VerifyServiceAdded(typeof(IRefreshTokenRepository), customRefreshRepository);
        }

        [Test()]
        public void Build_WithoutConfiguration_ReturnsOptionsWithInMemoryDataSourceServices()
        {
            EndpointerAuthenticationOptions options = _builder.Build();

            options.AddDataSourceServices(Services);
            VerifyServiceAdded(typeof(IUserRepository), typeof(InMemoryUserRepository));
            VerifyServiceAdded(typeof(IRefreshTokenRepository), typeof(InMemoryRefreshTokenRepository));
        }

        [Test()]
        public void Build_WithoutConfiguration_ReturnsOptionsWithRequireEmailVerificationFalse()
        {
            EndpointerAuthenticationOptions options = _builder.Build();

            Assert.IsFalse(options.RequireEmailVerification);
        }

        private void VerifyServiceAdded(Type inter, Type implem)
        {
            _mockServices.Verify(s => s.Add(It.Is<ServiceDescriptor>(s => s.ServiceType == inter && s.ImplementationType == implem)));
        }

        private void VerifyServiceAdded(Type inter, object implem)
        {
            _mockServices.Verify(s => s.Add(It.Is<ServiceDescriptor>(s => s.ServiceType == inter && s.ImplementationInstance == implem)));
        }
    }
}