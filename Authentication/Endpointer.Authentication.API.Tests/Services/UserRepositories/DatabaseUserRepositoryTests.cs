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
using Microsoft.Extensions.Logging;

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
            _repository = new DatabaseUserRepository<DefaultAuthenticationDbContext>(
                _context, 
                new Mock<ILogger<DatabaseUserRepository<DefaultAuthenticationDbContext>>>().Object);

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

        [Test]
        public async Task Update_WithExistingIdAndNewChanges_AppliesUpdate()
        {
            User user = new User() { Id = _userId, IsEmailVerified = false };
            _context.Users.Add(user);
            _context.SaveChanges();
            _context.Users.Local.Clear();

            await _repository.Update(_userId, u => u.IsEmailVerified = true);

            User savedUser = _context.Users.Find(_userId);
            Assert.IsTrue(savedUser.IsEmailVerified);
        }

        [Test]
        public async Task Update_WithExistingIdAndMultipleNewChanges_AppliesUpdate()
        {
            string username = "test";
            User user = new User() { Id = _userId, IsEmailVerified = false };
            _context.Users.Add(user);
            _context.SaveChanges();
            _context.Users.Local.Clear();

            await _repository.Update(_userId, u =>
            {
                u.IsEmailVerified = true;
                u.Username = username;
            });

            User savedUser = _context.Users.Find(_userId);
            Assert.IsTrue(savedUser.IsEmailVerified);
            Assert.AreEqual(username, savedUser.Username);
        }

        [Test]
        public async Task Update_WithExistingIdAndNoChanges_WorksButChangesNothing()
        {
            User user = new User() { Id = _userId, IsEmailVerified = false };
            _context.Users.Add(user);
            _context.SaveChanges();
            _context.Users.Local.Clear();

            await _repository.Update(_userId, u => u.IsEmailVerified = false);

            User savedUser = _context.Users.Find(_userId);
            Assert.IsFalse(savedUser.IsEmailVerified);
        }

        [Test]
        public void Update_WithNonExistingId_ThrowsException()
        {
            Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => _repository.Update(_userId, u => u.IsEmailVerified = true));
        }

        [Test]
        public void Update_WithFailure_ThrowsException()
        {
            _connection.Dispose();

            Assert.ThrowsAsync<DbUpdateException>(() => _repository.Update(_userId, u => u.IsEmailVerified = true));
        }

        protected override DefaultAuthenticationDbContext CreateDbContext(DbContextOptions options)
        {
            return new DefaultAuthenticationDbContext(options);
        }
    }
}