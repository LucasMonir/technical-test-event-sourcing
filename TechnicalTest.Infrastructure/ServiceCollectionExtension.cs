using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TechnicalTest.Application.Abstractions.Events;
using TechnicalTest.Application.Abstractions.Repositories;
using TechnicalTest.Infrastructure.Events;
using TechnicalTest.Infrastructure.Projections;
using TechnicalTest.Infrastructure.Repositories;

namespace TechnicalTest.Infrastructure
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddInfrastructurePersistence(this IServiceCollection services, string connectionString)
        {
            return services
                .AddDbContext(connectionString)
                .AddRepositories()
                .AddEventStore()
                .AddProjectionWorker();
        }
        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services.AddScoped<IPostRepository, PostRepository>();
        }

        private static IServiceCollection AddDbContext(this IServiceCollection services, string connectionString)
        {
            return services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(connectionString));
        }

        private static IServiceCollection AddEventStore(this IServiceCollection services)
        {
            return services.AddScoped<IEventStore, EfEventStore>();
        }

        private static IServiceCollection AddProjectionWorker(this IServiceCollection services)
        {
            return services.AddHostedService<PostProjectionWorker>()
                .AddHostedService<AuthorProjectionWorker>();
        }
    }
}
