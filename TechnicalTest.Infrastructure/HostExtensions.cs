using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TechnicalTest.Infrastructure.Persistence
{
    public static class HostExtensions
    {
        public static async Task InitializeDatabaseAsync(
            this IHost host,
            CancellationToken cancellationToken = default)
        {
            using var scope = host.Services.CreateScope();

            try
            {
                var context = scope.ServiceProvider
                    .GetRequiredService<AppDbContext>();

                await context.Database.EnsureCreatedAsync(cancellationToken);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
