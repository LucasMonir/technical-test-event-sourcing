using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TechnicalTest.Application.Abstractions.Events;
using TechnicalTest.Application.Abstractions.Persistence;
using TechnicalTest.Application.Abstractions.Repositories;
using TechnicalTest.Infrastructure.Persistence.Events;
using TechnicalTest.Infrastructure.Persistence.Repositories;
using TechnicalTest.Infrastructure.Persistence.Services;

namespace TechnicalTest.Infrastructure.Persistence
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddInfrastructurePersistence(this IServiceCollection services, string connectionString)
        {
            return services.AddRepositories()
                .AddDbContext(connectionString)
                .AddEventStore()
                .AddUnitOfWork();
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services.AddScoped<IPostRepository, PostRepository>()
                .AddScoped<IAuthorRepository, _sut>();

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

        private static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            return services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
