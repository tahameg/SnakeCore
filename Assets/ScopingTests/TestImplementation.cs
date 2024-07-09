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
            SnakeCoreApplicationRuntime.LogInfo("I am the first test implementation");
        }
    }
}