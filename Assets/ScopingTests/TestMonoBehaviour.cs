using SnakeCore.DI;
using VContainer;

namespace ScopingTests
{
    public class TestMonoBehaviour : InjectableMonoBehaviour
    {
        [Inject] private ITestInterface m_testInterface;
        
        private void Start()
        {
            m_testInterface.SayMyName();
        }
    }
}