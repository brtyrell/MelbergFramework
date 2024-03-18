using Microsoft.Extensions.DependencyInjection;

namespace MelbergFramework.Core.DependencyInjection;


public static class Overrider
{
    public static IServiceCollection OverrideWithLifetime<TImp, TTarget>(
            this IServiceCollection services
            )
        where TTarget : class, TImp
    {
        var currentRegistrationLifetime = services
            .Where(_ => _.ImplementationType == typeof(TTarget))
            .Select(_ => _.Lifetime)
            .First();

        return services.Replace<TImp, TTarget>(currentRegistrationLifetime);
    }

    public static IServiceCollection OverrideWithScoped<TImp, TTarget>(
            this IServiceCollection services)
        where TTarget : class, TImp
        =>
        services.Replace<TImp, TTarget>(ServiceLifetime.Scoped);


    public static IServiceCollection OverrideWithTransient<TImp, TTarget>(
            this IServiceCollection services)
        where TTarget : class, TImp
        =>
        services.Replace<TImp, TTarget>(ServiceLifetime.Transient);

    public static IServiceCollection OverrideWithSingleton<TImp, TTarget>(
            this IServiceCollection services
            )
        where TTarget : class, TImp
        =>
        services.Replace<TImp, TTarget>(ServiceLifetime.Singleton);

    private static IServiceCollection Replace<TImp, TTarget>(
            this IServiceCollection services,
            ServiceLifetime lifetime
            )
        where TTarget : class, TImp
    {

        var desciptor = services
            .Where(_ => _.ImplementationType == typeof(TImp)).FirstOrDefault();
        if (desciptor != null)
        {
            services.Remove(desciptor);
        }
        services.Add(ServiceDescriptor.Describe(typeof(TImp), typeof(TTarget), lifetime));
        return services;
    }


}
