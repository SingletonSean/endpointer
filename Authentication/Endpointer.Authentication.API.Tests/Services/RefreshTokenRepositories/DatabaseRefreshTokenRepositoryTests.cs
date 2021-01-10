using NUnit.Framework;
using Endpointer.Authentication.API.Services.RefreshTokenRepositories;
using System;
using Endpointer.Authentication.API.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Endpointer.Authentication.API.Models;
using Moq;
using Microsoft.Data.Sqlite;
using Endpointer.API.Tests.Services;
using Microsoft.Extensions.Logging;

namespace Endpointer.Authentication.API.Tests.Services.RefreshTokenRepositories
{
    [TestFixture]
    public class DatabaseRefreshTokenRepositoryTests : DbContextTests<DefaultAuthenticationDbContext>
    {
        private DatabaseRefreshTokenRepository<DefaultAuthenticationDbContext> _repository;

        private string _token;

        [SetUp]
        public void SetUp()
        {
            _repository = new DatabaseRefreshTokenRepository<DefaultAuthenticationDbContext>(
                _context, 
                new Mock<ILogger<DatabaseRefreshTokenRepository<DefaultAuthenticationDbContext>>>().Object);

            _token = "123token123";
        }

        [Test]
        public async Task Create_WithSuccess_SavesRefreshToken()
        {
            RefreshToken refreshToken = new RefreshToken() { Token = _token };

            await _repository.Create(refreshToken);

            RefreshToken savedToken = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == refreshToken.Token);
            Assert.IsNotNull(savedToken);
        }

        [Test]
        public void Create_WithFailure_ThrowsException()
        {
            _connection.Dispose();

            Assert.ThrowsAsync<DbUpdateException>(() => _repository.Create(new RefreshToken()));
        }

        [Test]
        public async Task DeleteByToken_WithExistingToken_RemovesToken()
        {
            RefreshToken refreshToken = new RefreshToken() { Token = _token };
            _context.RefreshTokens.Add(refreshToken);
            _context.SaveChanges();

            await _repository.DeleteByToken(refreshToken.Token);

            RefreshToken savedToken = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == refreshToken.Token);
            Assert.IsNull(savedToken);
        }

        [Test]
        public void DeleteByToken_WithNonExistingToken_DoesNotThrow()
        {
            RefreshToken refreshToken = new RefreshToken() { Token = _token };
            _context.RefreshTokens.Add(refreshToken);
            _context.SaveChanges();

            Assert.DoesNotThrowAsync(() => _repository.DeleteByToken(It.IsAny<string>()));
        }

        [Test]
        public void DeleteByToken_WithFailure_ThrowsException()
        {
            _connection.Dispose();

            Assert.ThrowsAsync<SqliteException>(() => _repository.DeleteByToken(_token));
        }

        [Test]
        public async Task DeleteById_WithExistingId_RemovesToken()
        {
            Guid id = Guid.NewGuid();
            RefreshToken refreshToken = new RefreshToken() { Id = id };
            _context.RefreshTokens.Add(refreshToken);
            _context.SaveChanges();

            await _repository.DeleteById(id);

            RefreshToken savedToken = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Id == refreshToken.Id);
            Assert.IsNull(savedToken);
        }

        [Test]
        public void DeleteById_WithNonExistingToken_DoesNotThrow()
        {
            RefreshToken refreshToken = new RefreshToken() { Id = Guid.NewGuid() };
            _context.RefreshTokens.Add(refreshToken);
            _context.SaveChanges();

            Assert.DoesNotThrowAsync(() => _repository.DeleteById(Guid.NewGuid()));
        }

        [Test]
        public void DeleteById_WithFailure_ThrowsException()
        {
            _connection.Dispose();

            Assert.ThrowsAsync<SqliteException>(() => _repository.DeleteById(It.IsAny<Guid>()));
        }

        [Test]
        public async Task DeleteAll_WithExistingUserTokens_RemovesTokens()
        {
            Guid userId = Guid.NewGuid();
            RefreshToken refreshToken1 = new RefreshToken() { UserId = userId };
            RefreshToken refreshToken2 = new RefreshToken() { UserId = userId };
            _context.RefreshTokens.Add(refreshToken1);
            _context.RefreshTokens.Add(refreshToken2);
            _context.SaveChanges();

            await _repository.DeleteAll(userId);

            RefreshToken savedToken = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.UserId == userId);
            Assert.IsNull(savedToken);
        }

        [Test]
        public void DeleteAll_WithNonExistingUserToken_DoesNotThrow()
        {
            RefreshToken refreshToken = new RefreshToken() { UserId = Guid.NewGuid() };
            _context.RefreshTokens.Add(refreshToken);
            _context.SaveChanges();

            Assert.DoesNotThrowAsync(() => _repository.DeleteAll(Guid.NewGuid()));
        }

        [Test]
        public void DeleteAll_WithFailure_ThrowsException()
        {
            _connection.Dispose();

            Assert.ThrowsAsync<SqliteException>(() => _repository.DeleteAll(It.IsAny<Guid>()));
        }

        [Test]
        public async Task GetByToken_WithExistingToken_ReturnsToken()
        {
            RefreshToken refreshToken = new RefreshToken() { Token = _token };
            _context.RefreshTokens.Add(refreshToken);
            _context.SaveChanges();

            RefreshToken savedToken = await _repository.GetByToken(refreshToken.Token);

            Assert.IsNotNull(savedToken);
        }

        [Test]
        public async Task GetByToken_WithNonExistingToken_ReturnsNull()
        {
            RefreshToken refreshToken = new RefreshToken() { Token = _token };
            _context.RefreshTokens.Add(refreshToken);
            _context.SaveChanges();

            RefreshToken savedToken = await _repository.GetByToken(It.IsAny<string>());

            Assert.IsNull(savedToken);
        }

        [Test]
        public void GetByToken_WithFailure_ThrowsException()
        {
            _connection.Dispose();

            Assert.ThrowsAsync<SqliteException>(() => _repository.GetByToken(_token));
        }

        protected override DefaultAuthenticationDbContext CreateDbContext(DbContextOptions options)
        {
            return new DefaultAuthenticationDbContext(options);
        }
    }
}