using Endpointer.Authentication.API.Contexts;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Endpointer.Authentication.API.Tests.Services
{
    [TestFixture]
    public class DbContextTests
    {
        protected DbConnection _connection;
        protected DefaultAuthenticationDbContext _context;

        [SetUp]
        public void BaseSetUp()
        {
            _connection = new SqliteConnection("Data Source=:memory:");
            _connection.Open();
            _context = new DefaultAuthenticationDbContext(new DbContextOptionsBuilder()
                .UseSqlite(_connection)
                .Options);
            
            _context.Database.EnsureCreated();
        }

        [TearDown]
        public void BaseTearDown()
        {
            _connection.Dispose();
            _context.Dispose();
        }
    }
}
