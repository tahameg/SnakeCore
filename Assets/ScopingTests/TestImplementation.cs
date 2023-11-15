using TahaCore;

namespace ScopingTests
{
    [ApplicationRuntimeRegistry(LifetimeType.Singleton, typeof(ITestInterface))]
    public class TestImplementation : ITestInterface
    {
        public void SayMyName()
        {
            TahaCoreApplicationRuntime.LogInfo("My name is TestImplementation");
        }
    }
}