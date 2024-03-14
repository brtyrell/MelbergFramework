using MelbergFramework.Core.HealthCheck;

namespace Application.Example;

public class TestHealthCheck : IHealthCheck
{
    public string Name => "test";

    public Task<bool> IsOk(CancellationToken token)
    {
        return Task.FromResult(true);
    }
}
