using SnakeCore.DI;
using SnakeCore.DI.ConfigConditions;

namespace ScopingTests
{
    [ApplicationRuntimeRegistry(LifetimeType.Singleton, typeof(ITestInterface))]
    [KeyExistenceConfigCondition("TAHA_TEST", "inject_second_test", ExistenceCompareType.Exists)]
    public class TestImplementation2 : ITestInterface
    {
        public void SayMyName()
        {
            SnakeCoreApplicationRuntime.LogInfo("I am the second test implementation");
        }
    }
}