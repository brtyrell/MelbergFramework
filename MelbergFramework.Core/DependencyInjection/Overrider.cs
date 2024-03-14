using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MelbergFramework.Core.DependencyInjection;


public static class Overrider
{
    public static IServiceCollection OverrideWithLifetime(
            this IServiceCollection services,
            Type imp,
            Type target
            )
    {
        var currentRegistrationLifetime = services
            .Where(_ => _.ImplementationType == target)
            .Select(_ => _.Lifetime)
            .First();

        return services.Replace(imp,target,currentRegistrationLifetime);
    }

    public static IServiceCollection OverrideWithScoped(
            this IServiceCollection services,
            Type imp,
            Type target
            ) =>
        services.Replace(imp, target, ServiceLifetime.Scoped);


    public static IServiceCollection OverrideWithTransient(
            this IServiceCollection services,
            Type imp,
            Type target
            ) =>
        services.Replace(imp,target,ServiceLifetime.Transient);

    public static IServiceCollection OverrideWithSingleton(
            this IServiceCollection services,
            Type imp,
            Type target
            ) =>
        services.Replace(imp, target, ServiceLifetime.Singleton);

    private static IServiceCollection Replace(
            this IServiceCollection services,
            Type imp,
            Type target,
            ServiceLifetime lifetime 
            )
    {
       services.Replace(ServiceDescriptor.Describe(imp,target,lifetime)) ;
       return services;
    }


}
