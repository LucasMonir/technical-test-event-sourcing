using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TechnicalTest.Infrastructure;

namespace TechnicalTest.E2E.Test
{
    public class TechnicalTestWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private SqliteConnection _connection = null!;

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureLogging(logging =>
            {
                logging.Services.RemoveAll<ILoggerProvider>();
            });
            return base.CreateHost(builder);
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

        public async Task InitializeAsync()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            await _connection.OpenAsync();

            await using var scope = Services.CreateAsyncScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await db.Database.EnsureCreatedAsync();
        }

        public new async Task DisposeAsync()
        {
            await _connection.CloseAsync();
            await _connection.DisposeAsync();
            await base.DisposeAsync();
        }

        protected override void Dispose(bool disposing)
        {
        }
    }
}
