using Microsoft.Extensions.DependencyInjection;

namespace MelbergFramework.Core.Time;

public static class ClockModule
{
    public static void RegisterClock(IServiceCollection services)
    {
        services.AddSingleton<IClock,Clock>();
    }
}
