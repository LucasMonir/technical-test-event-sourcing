using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TechnicalTest.Infrastructure.Persistence;

namespace TechnicalTest.TestHelpers.Database
{
    internal class TestDbContextFactory : IDisposable
    {
        private readonly SqliteConnection _connection;
        public AppDbContext Context { get; }

        public TestDbContextFactory()
        {
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(_connection)
                .Options;

            Context = new AppDbContext(options);
            Context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            Context?.Dispose();
            _connection?.Dispose();
        }
    }
}
