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

namespace Endpointer.Authentication.API.Firebase.Tests.Extensions
{
    [TestFixture()]
    public class EndpointerAuthenticationOptionsBuilderFirebaseExtensionsTests
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
        public void Build_WithFirebaseDataSource_ReturnsOptionsWithFirebaseDataSourceServices()
        {
            FirebaseClient client = new FirebaseClient(string.Empty);

            EndpointerAuthenticationOptions options = _builder.WithFirebaseDataSource(client).Build();

            options.AddDataSourceServices(Services);
            VerifyServiceAdded(typeof(FirebaseClient), client);
            VerifyServiceAdded(typeof(IUserRepository), typeof(FirebaseUserRepository));
            VerifyServiceAdded(typeof(IRefreshTokenRepository), typeof(FirebaseRefreshTokenRepository));
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