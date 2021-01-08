using NUnit.Framework;
using Endpointer.Accounts.API.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Endpointer.Accounts.API.Tests.Models
{
    [TestFixture()]
    public class EndpointerAccountsOptionsBuilderTests
    {
        private EndpointerAccountsOptionsBuilder _builder;

        [SetUp]
        public void Setup()
        {
            _builder = new EndpointerAccountsOptionsBuilder();
        }

        [Test()]
        public void Build_WithDatabase_ReturnsOptionsWithUseDatabaseTrue()
        {
            EndpointerAccountsOptions options = _builder.WithDatabase().Build();

            Assert.IsTrue(options.UseDatabase);
        }

        [Test()]
        public void Build_WithDatabase_ReturnsOptionsWithDatabaseServices()
        {
            EndpointerAccountsOptions options = _builder.WithDatabase().Build();

            Assert.IsNotNull(options.AddDbContext);
            Assert.IsNotNull(options.AddDbAccountRepository);
        }

        [Test()]
        public void Build_WithoutConfiguration_ReturnsOptionsWithUseDatabaseFalse()
        {
            EndpointerAccountsOptions options = _builder.Build();

            Assert.IsFalse(options.UseDatabase);
        }

        [Test()]
        public void Build_WithoutConfiguration_ReturnsOptionsWithoutDatabaseServices()
        {
            EndpointerAccountsOptions options = _builder.Build();

            Assert.IsNull(options.AddDbContext);
            Assert.IsNull(options.AddDbAccountRepository);
        }
    }
}