using SnakeCore.DI;
using SnakeCore.DI.ConfigConditions;

namespace ScopingTests
{
    [ApplicationRuntimeRegistry(LifetimeType.Singleton, typeof(ITestInterface))]
    [KeyExistenceConfigCondition("TAHA_TEST", "inject_first_test", ExistenceCompareType.Exists)]
    public class TestImplementation : ITestInterface
    {
        public void SayMyName()
        {
            SakeCoreApplicationRuntime.LogInfo("I am the first test implementation");
        }
    }
}