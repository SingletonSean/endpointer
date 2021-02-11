using NUnit.Framework;
using Endpointer.Authentication.API.Firebase.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using Endpointer.Authentication.API.Models;
using Firebase.Database;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Endpointer.Authentication.API.Services.UserRepositories;
using Endpointer.Authentication.API.Services.RefreshTokenRepositories;
using Endpointer.Authentication.API.Firebase.Services.RefreshTokenRepositories;
using Endpointer.API.Tests.ServiceCollections;

namespace Endpointer.Authentication.API.Firebase.Tests.Extensions
{
    [TestFixture()]
    public class EndpointerAuthenticationOptionsBuilderFirebaseExtensionsTests
    {
        private EndpointerAuthenticationOptionsBuilder _builder;

        private MockServiceCollectionTests _mockServices;

        private IServiceCollection Services => _mockServices.MockServices.Object;

        [SetUp]
        public void Setup()
        {
            _builder = new EndpointerAuthenticationOptionsBuilder();

            _mockServices = new MockServiceCollectionTests();
            _mockServices.SetUp();
        }

        [Test()]
        public void Build_WithFirebaseDataSource_ReturnsOptionsWithFirebaseDataSourceServices()
        {
            FirebaseClient client = new FirebaseClient(string.Empty);

            EndpointerAuthenticationOptions options = _builder.WithFirebaseDataSource(client).Build();

            options.AddDataSourceServices(Services);
            _mockServices.VerifyServiceAdded(typeof(FirebaseClient), client);
            _mockServices.VerifyServiceAdded(typeof(IUserRepository), typeof(FirebaseUserRepository));
            _mockServices.VerifyServiceAdded(typeof(IRefreshTokenRepository), typeof(FirebaseRefreshTokenRepository));
        }
    }
}