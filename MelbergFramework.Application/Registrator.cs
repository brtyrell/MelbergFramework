using Microsoft.Extensions.DependencyInjection;

namespace MelbergFramework.Application;

public abstract class Registrator
{
    public abstract void 
        RegisterServices(IServiceCollection services);
}
