namespace MelbergFramework.Core.HealthCheck;

public interface IHealthCheckChecker
{
    Task<bool> IsOk();
}
