using TahaCore;
using TahaCore.Runtime.DI;
using TahaCore.Runtime.DI.ConfigConditions;

namespace ScopingTests
{
    [ApplicationRuntimeRegistry(LifetimeType.Singleton, typeof(ITestInterface))]
    [KeyExistenceConfigCondition("TAHA_TEST", "inject_first_test", ExistenceType.Exists)]
    public class TestImplementation : ITestInterface
    {
        public void SayMyName()
        {
            TahaCoreApplicationRuntime.LogInfo("I am the first test implementation");
        }
    }
}