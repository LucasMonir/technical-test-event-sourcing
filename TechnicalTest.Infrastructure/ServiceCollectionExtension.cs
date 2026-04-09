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
            AddDbContext(services, connectionString);

            return services;
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
        }

        private static void AddDbContext(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(connectionString));
        }
    }
}
