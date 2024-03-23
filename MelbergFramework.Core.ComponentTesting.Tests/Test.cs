using Microsoft.VisualStudio.TestTools.UnitTesting;
using LightBDD.MsTest3;
using LightBDD.Framework.Scenarios;

namespace MelbergFramework.Core.ComponentTesting.Tests;
[TestClass]
public partial class Test : BaseTest
{
    [Scenario]
    [TestMethod]
    public async Task TestMethod()
    {
        await Runner.RunScenarioAsync(
            _ => Set_time(),
            _ => Time_set()
                );

    }
}
