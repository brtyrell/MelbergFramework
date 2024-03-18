using Application.Example;
using MelbergFramework.Core.ComponentTesting;
using MelbergFramework.Core.DependencyInjection;
using MelbergFramework.Core.HealthCheck;
using MelbergFramework.Core.Time;
using MelbergFramework.Application;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var app = MelbergHost
            .CreateHost()
            .AddServices(_ =>
                {
                    ClockModule.RegisterClock(_);
                })
            .AddServices(_ => 
                {
                    _.AddTransient<IHealthCheck, TestHealthCheck>();                              
                    _.AddSingleton<MockClock>();
                    _.OverrideWithSingleton<IClock,MockClock>();
                })
            .AddControllers()
            .Build();

        var isGood = await app
            .Services.GetRequiredService<IHealthCheckChecker>().IsOk();


        var mock = (MockClock)app.Services.GetRequiredService<IClock>();

        mock.NewCurrentTime = DateTime.MaxValue;

        var clock = app.Services.GetRequiredService<IClock>();

        Console.WriteLine(clock.GetUtcNow == mock.GetUtcNow);
        await app.RunAsync();
    }
}
