using NUnit.Framework;
using TahaCore.Tests.Runtime;

namespace TahaCore.Tests.PlayMode.TestBoot
{
    public class RuntimeTestBootTest : RuntimeTestBoot
    {
        [Test]
        public void InitializationTest()
        {
            Assert.NotNull(Runtime);
            Assert.NotNull(Runtime.Container);
        }
    }
}