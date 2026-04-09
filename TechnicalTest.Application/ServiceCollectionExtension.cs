using Microsoft.Extensions.DependencyInjection;
using TechnicalTest.Application.Abstractions.Services;
using TechnicalTest.Application.Services;

namespace TechnicalTest.Application
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IPostQueryService, PostQueryService>();
            services.AddScoped<IPostCommandHandler, PostCommandHandler>();

            return services;
        }
    }
}
