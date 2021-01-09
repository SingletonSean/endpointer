using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Data.Common;

namespace Endpointer.API.Tests.Services
{
    [TestFixture]
    public abstract class DbContextTests<TDbContext> where TDbContext : DbContext
    {
        protected DbConnection _connection;
        protected TDbContext _context;

        [SetUp]
        public void BaseSetUp()
        {
            _connection = new SqliteConnection("Data Source=:memory:");
            _connection.Open();
            _context = CreateDbContext(new DbContextOptionsBuilder()
                .UseSqlite(_connection)
                .Options);
            
            _context.Database.EnsureCreated();
        }

        protected abstract TDbContext CreateDbContext(DbContextOptions options);

        [TearDown]
        public void BaseTearDown()
        {
            _connection.Dispose();
            _context.Dispose();
        }
    }
}
