using NUnit.Framework;
using Endpointer.Accounts.API.Models;
using Moq;
using Microsoft.Extensions.DependencyInjection;
using Endpointer.API.Tests.ServiceCollections;
using Endpointer.Accounts.API.Services.AccountRepositories;
using Endpointer.Accounts.API.Contexts;
using System;

namespace Endpointer.Accounts.API.Tests.Models
{
    [TestFixture()]
    public class EndpointerAccountsOptionsBuilderTests
    {
        private EndpointerAccountsOptionsBuilder _builder;

        private MockServiceCollectionTests _mockServicesTests;
        private Mock<IServiceCollection> MockServices => _mockServicesTests.MockServices;
        private IServiceCollection Services => MockServices.Object;

        [SetUp]
        public void Setup()
        {
            _builder = new EndpointerAccountsOptionsBuilder();

            _mockServicesTests = new MockServiceCollectionTests();
            _mockServicesTests.SetUp();
        }

        [Test()]
        public void Build_WithEntityFrameworkDataSource_ReturnsOptionsWithEntityFrameworkDataSourceServices()
        {
            EndpointerAccountsOptions options = _builder.WithEntityFrameworkDataSource().Build();

            options.AddDataSourceServices(Services);
            _mockServicesTests.VerifyServiceAdded(typeof(DefaultAccountDbContext), It.IsAny<DefaultAccountDbContext>());
            _mockServicesTests.VerifyServiceAdded(typeof(IAccountRepository), typeof(DatabaseAccountRepository<DefaultAccountDbContext>));
        }

        [Test()]
        public void Build_WithCustomDataSource_ReturnsOptionsWithCustomDataSourceServices()
        {
            IAccountRepository customAccountRepository = new Mock<IAccountRepository>().Object;
            Action<IServiceCollection> addCustomDataSource = (s) =>
            {
                s.AddSingleton(customAccountRepository);
            };

            EndpointerAccountsOptions options = _builder.WithCustomDataSource(addCustomDataSource).Build();

            options.AddDataSourceServices(Services);
            _mockServicesTests.VerifyServiceAdded(typeof(IAccountRepository), customAccountRepository);
        }

        [Test()]
        public void Build_WithoutConfiguration_ReturnsOptionsWithInMemoryDataSourceServices()
        {
            EndpointerAccountsOptions options = _builder.Build();

            options.AddDataSourceServices(Services);
            _mockServicesTests.VerifyServiceAdded(typeof(IAccountRepository), typeof(InMemoryAccountRepository));
        }
    }
}