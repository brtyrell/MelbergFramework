using Application.Example;
using MelbergFramework.Core.ComponentTesting;
using MelbergFramework.Core.DependencyInjection;
using MelbergFramework.Core.HealthCheck;
using MelbergFramework.Core.Time;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        HealthCheckModule.RegisterHealthCheckChecker(builder.Services);
        ClockModule.RegisterClock(builder.Services);
        builder.Services.AddSingleton<MockClock, MockClock>();
        builder.Services.AddTransient<IHealthCheck, TestHealthCheck>();
        builder.Services.OverrideWithSingleton(typeof(IClock),typeof(MockClock));
        var app = builder.Build();

        var clock = app.Services.GetService<IClock>();

        var isGood = await app
            .Services.GetRequiredService<IHealthCheckChecker>().IsOk();

        app.MapGet("/", () => "Hello World!");

        app.Run();
    }
}
