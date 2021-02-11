using NUnit.Framework;
using Endpointer.Accounts.API.Firebase.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using Endpointer.Accounts.API.Models;
using Endpointer.API.Tests.ServiceCollections;
using Firebase.Database;
using Endpointer.Accounts.API.Services.AccountRepositories;
using Endpointer.Accounts.API.Firebase.Services.AccountRepositories;

namespace Endpointer.Authentication.API.Firebase.Tests.Extensions
{
    [TestFixture()]
    public class EndpointerAccountsOptionsBuilderFirebaseExtensionsTests
    {
        private EndpointerAccountsOptionsBuilder _builder;

        private MockServiceCollectionTests _mockServices;

        [SetUp]
        public void SetUp()
        {
            _builder = new EndpointerAccountsOptionsBuilder();

            _mockServices = new MockServiceCollectionTests();
            _mockServices.SetUp();
        }

        [Test()]
        public void Build_WithFirebaseDataSource_ReturnsOptionsWithFirebaseDataSourceServices()
        {
            FirebaseClient client = new FirebaseClient(string.Empty);

            EndpointerAccountsOptions options = _builder.WithFirebaseDataSource(client).Build();

            options.AddDataSourceServices(_mockServices.MockServices.Object);
            _mockServices.VerifyServiceAdded(typeof(FirebaseClient), client);
            _mockServices.VerifyServiceAdded(typeof(IAccountRepository), typeof(FirebaseAccountRepository));
        }
    }
}