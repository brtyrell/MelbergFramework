using MelbergFramework.Application;
using MelbergFramework.Core.Time;
using MelbergFramework.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MelbergFramework.Core.ComponentTesting.Tests;

public partial class BaseTest : BaseTestFrame
{
    public BaseTest()
    {
        App = MelbergHost
            .CreateHost<TestRegistrator>()
            .AddServices( (_) => 
                {
                   _.OverrideWithSingleton<IClock,MockClock>();
                })
            .Build();
    }

    public async Task Set_time()
    {
        var clockMock = (MockClock) GetClass<IClock>();
        clockMock.NewCurrentTime = DateTime.MinValue;
    }

    public async Task Time_set()
    {
        var small = GetClass<ISmallTest>();
        Assert.IsTrue(small.GetTime() == DateTime.MinValue);
    }

}

public class TestRegistrator : Registrator
{
    public override void RegisterServices(IServiceCollection services)
    {
        services.AddTransient<ISmallTest,SmallTest>();
        ClockModule.RegisterClock(services);
    }
}

public interface ISmallTest
{
    DateTime GetTime();
}
public class SmallTest : ISmallTest
{
    private IClock _clock;
    public SmallTest(IClock clock)
    {
        _clock = clock;
    }

    public DateTime GetTime() =>
        _clock.GetUtcNow();
}
