using TahaCore;
using TahaCore.Runtime.DI;
using VContainer;

namespace ScopingTests
{
    [ApplicationRuntimeRegistry(LifetimeType.Singleton)]
    public class TestInjection
    {
        [Inject]
        public TestInjection(ITestInterface testInterface)
        {
            testInterface.SayMyName();
        }
    }
}