using MelbergFramework.Application;
using MelbergFramework.Core.Time;

namespace Application.Example;

public class DemoRegistrator : Registrator
{
    public DemoRegistrator() { }

    public override void RegisterServices(IServiceCollection services)
    {
        ClockModule.RegisterClock(services);
    }

}
