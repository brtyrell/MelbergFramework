namespace MelbergFramework.Core.HealthCheck;

public interface IHealthCheck
{
    Task<bool> IsOk(CancellationToken token);
    string Name {get;}
}
