namespace MelbergFramework.Core.HealthCheck;

public abstract class HealthCheck : IHealthCheck
{
    public abstract string Name {get;}
    public abstract Task<bool> IsOk(CancellationToken token);
}
