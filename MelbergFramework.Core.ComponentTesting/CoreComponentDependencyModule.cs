using MelbergFramework.Application;
using Microsoft.Extensions.DependencyInjection;

namespace MelbergFramework.Core.ComponentTesting;

public static class CoreComponentDependencyModule
{
    public static IServiceCollection PrepareApplication(IServiceCollection services)
    {
        services.Configure<ApplicationConfiguration>(_ => 
        {
            _.Name = "Test";
            _.Version = "1";

        });

        return services;
    }
}
