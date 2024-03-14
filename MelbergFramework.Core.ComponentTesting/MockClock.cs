using MelbergFramework.Core.Time;

namespace MelbergFramework.Core.ComponentTesting;

public class MockClock : IClock
{
    public DateTime NewCurrentTime {get; set;}
    public DateTime GetUtcNow() => NewCurrentTime;
}
