using NUnit.Framework;
using Endpointer.Accounts.API.Services.AccountRepositories;
using System;
using Endpointer.API.Tests.Services;
using Endpointer.Accounts.API.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Endpointer.Core.API.Models;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using Moq;

namespace Endpointer.Accounts.API.Tests.Services.AccountRepositories
{
    [TestFixture()]
    public class DatabaseAccountRepositoryTests : DbContextTests<DefaultAccountDbContext>
    {
        private DatabaseAccountRepository<DefaultAccountDbContext> _repository;

        private Guid _userId;

        [SetUp]
        public void Setup()
        {
            _repository = new DatabaseAccountRepository<DefaultAccountDbContext>(
                _context, 
                new Mock<ILogger<DatabaseAccountRepository<DefaultAccountDbContext>>>().Object);

            _userId = Guid.NewGuid();
        }

        [Test]
        public async Task GetById_WithExistingId_ReturnsAccount()
        {
            User user = new User() { Id = _userId };
            _context.Accounts.Add(user);
            _context.SaveChanges();

            User savedUser = await _repository.GetById(user.Id);

            Assert.IsNotNull(savedUser);
        }

        [Test]
        public async Task GetById_WithNonExistingId_ReturnsNull()
        {
            User user = new User() { Id = _userId };
            _context.Accounts.Add(user);
            _context.SaveChanges();

            User savedUser = await _repository.GetById(Guid.NewGuid());

            Assert.IsNull(savedUser);
        }

        [Test]
        public void GetByUsername_WithFailure_ThrowsException()
        {
            _connection.Dispose();

            Assert.ThrowsAsync<SqliteException>(() => _repository.GetById(Guid.NewGuid()));
        }

        protected override DefaultAccountDbContext CreateDbContext(DbContextOptions options)
        {
            return new DefaultAccountDbContext(options);
        }
    }
}