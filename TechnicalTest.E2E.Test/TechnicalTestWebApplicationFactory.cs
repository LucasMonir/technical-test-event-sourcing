using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using TechnicalTest.Infrastructure.Persistence;

namespace TechnicalTest.E2E.Test
{
    public class TechnicalTestWebApplicationFactory : WebApplicationFactory<Program>
    {
        private SqliteConnection? connection;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();

                services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlite(connection));

                using var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.EnsureCreated();
            });
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                connection?.Close();
                connection?.Dispose();
            }
        }
    }
}
