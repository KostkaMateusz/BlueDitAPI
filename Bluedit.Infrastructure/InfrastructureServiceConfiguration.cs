using Bluedit.Infrastructure.StorageService;
using Microsoft.Extensions.DependencyInjection;

namespace Bluedit.Infrastructure;

public static class InfrastructureServiceConfiguration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<IAzureStorageService, AzureStorageService>();

        return services;
    }
}