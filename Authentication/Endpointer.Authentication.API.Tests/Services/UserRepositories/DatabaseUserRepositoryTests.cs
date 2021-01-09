using NUnit.Framework;
using Endpointer.Authentication.API.Services.UserRepositories;
using System;
using Endpointer.Authentication.API.Contexts;
using System.Threading.Tasks;
using Endpointer.Core.API.Models;
using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Endpointer.API.Tests.Services;

namespace Endpointer.Authentication.API.Tests.Services.UserRepositories
{
    [TestFixture]
    public class DatabaseUserRepositoryTests : DbContextTests<DefaultAuthenticationDbContext>
    {
        private DatabaseUserRepository<DefaultAuthenticationDbContext> _repository;

        private Guid _userId;

        [SetUp]
        public void SetUp()
        {
            _repository = new DatabaseUserRepository<DefaultAuthenticationDbContext>(_context);

            _userId = Guid.NewGuid();
        }

        [Test]
        public async Task Create_WithSuccess_SavesUser()
        {
            User user = new User() { Id = _userId };

            await _repository.Create(user);

            User savedUser = await _context.Users.FindAsync(_userId);
            Assert.IsNotNull(savedUser);
        }

        [Test]
        public void Create_WithFailure_ThrowsException()
        {
            _connection.Dispose();

            Assert.ThrowsAsync<DbUpdateException>(() => _repository.Create(new User()));
        }

        [Test]
        public async Task GetByEmail_WithExistingEmail_ReturnsUser()
        {
            string email = "test@gmail.com";
            User user = new User() { Id = _userId, Email = email };
            _context.Users.Add(user);
            _context.SaveChanges();

            User savedUser = await _repository.GetByEmail(email);

            Assert.IsNotNull(savedUser);
        }

        [Test]
        public async Task GetByEmail_WithNonExistingEmail_ReturnsNull()
        {
            string email = "test@gmail.com";
            User user = new User() { Id = _userId, Email = email };
            _context.Users.Add(user);
            _context.SaveChanges();

            User savedUser = await _repository.GetByEmail(It.IsAny<string>());

            Assert.IsNull(savedUser);
        }

        [Test]
        public void GetByEmail_WithFailure_ThrowsException()
        {
            _connection.Dispose();

            Assert.ThrowsAsync<SqliteException>(() => _repository.GetByEmail(string.Empty));
        }

        [Test]
        public async Task GetById_WithExistingId_ReturnsUser()
        {
            User user = new User() { Id = _userId };
            _context.Users.Add(user);
            _context.SaveChanges();

            User savedUser = await _repository.GetById(_userId);

            Assert.IsNotNull(savedUser);
        }

        [Test]
        public async Task GetById_WithNonExistingId_ReturnsNull()
        {
            User user = new User() { Id = _userId };
            _context.Users.Add(user);
            _context.SaveChanges();

            User savedUser = await _repository.GetById(It.IsAny<Guid>());

            Assert.IsNull(savedUser);
        }

        [Test]
        public void GetById_WithFailure_ThrowsException()
        {
            _connection.Dispose();

            Assert.ThrowsAsync<SqliteException>(() => _repository.GetById(_userId));
        }

        [Test]
        public async Task GetByUsername_WithExistingUsername_ReturnsUser()
        {
            string username = "test";
            User user = new User() { Id = _userId, Username = username };
            _context.Users.Add(user);
            _context.SaveChanges();

            User savedUser = await _repository.GetByUsername(username);

            Assert.IsNotNull(savedUser);
        }

        [Test]
        public async Task GetByUsername_WithNonExistingUsername_ReturnsNull()
        {
            string username = "test";
            User user = new User() { Id = _userId, Username = username };
            _context.Users.Add(user);
            _context.SaveChanges();

            User savedUser = await _repository.GetByUsername(It.IsAny<string>());

            Assert.IsNull(savedUser);
        }

        [Test]
        public void GetByUsername_WithFailure_ThrowsException()
        {
            _connection.Dispose();

            Assert.ThrowsAsync<SqliteException>(() => _repository.GetByUsername(string.Empty));
        }

        protected override DefaultAuthenticationDbContext CreateDbContext(DbContextOptions options)
        {
            return new DefaultAuthenticationDbContext(options);
        }
    }
}