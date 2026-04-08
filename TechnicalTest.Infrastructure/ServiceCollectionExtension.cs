using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TechnicalTest.Application.Abstractions.Repositories;
using TechnicalTest.Infrastructure.Persistence.Repositories;

namespace TechnicalTest.Infrastructure.Persistence
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddInfrastructurePersistence(this IServiceCollection services, string connectionString)
        {
            AddRepositories(services);
            AddDbContext(services);

            return services;
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
        }

        private static void AddDbContext(IServiceCollection services)
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(connection));

            using var scope = services.BuildServiceProvider().CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            context.Database.EnsureCreated();
        }
    }
}
