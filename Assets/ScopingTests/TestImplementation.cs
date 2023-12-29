using TahaCore;
using TahaCore.DI;
using TahaCore.DI.ConfigConditions;
using TahaCore.Runtime.DI;

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