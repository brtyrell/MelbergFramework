namespace MelbergFramework.Core.Time;

public class Clock : IClock
{
    public DateTime GetUtcNow() => DateTime.UtcNow;
}
