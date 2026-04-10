using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TechnicalTest.Infrastructure.Persistence
{
    public static class HostExtensions
    {
        public static async Task InitializeDatabaseAsync(this IHost host)
        {
            using var scope = host.Services.CreateScope();

            var context = scope.ServiceProvider
                .GetRequiredService<AppDbContext>();

            await context.Database.MigrateAsync();
        }
    }
}
