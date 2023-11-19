using TahaCore;
using TahaCore.DI;
using TahaCore.DI.ConfigConditions;

namespace ScopingTests
{
    [ApplicationRuntimeRegistry(LifetimeType.Singleton, typeof(ITestInterface))]
    [KeyExistenceConfigCondition("TAHA_TEST", "inject_second_test", ExistenceType.Exists)]
    public class TestImplementation2 : ITestInterface
    {
        public void SayMyName()
        {
            TahaCoreApplicationRuntime.LogInfo("I am the second test implementation");
        }
    }
}