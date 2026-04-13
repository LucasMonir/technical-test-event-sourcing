using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TechnicalTest.Infrastructure;

namespace TechnicalTest.E2E.Test
{
    public class TechnicalTestWebApplicationFactory : WebApplicationFactory<Program>
    {
        private readonly SqliteConnection _connection;

        public TechnicalTestWebApplicationFactory()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<DbContextOptions<AppDbContext>>();

                services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlite(_connection, sqliteOptions =>
                    {
                        sqliteOptions.MaxBatchSize(1);
                    }));
            });
        }

        public void InitializeDatabase()
        {
            using var scope = Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.EnsureCreated();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _connection.Close();
                _connection.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
