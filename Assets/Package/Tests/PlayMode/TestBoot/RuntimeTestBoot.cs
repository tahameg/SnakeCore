using NUnit.Framework;

namespace TahaCore.Tests.PlayMode.TestBoot
{
    [TestFixture]
    public class RuntimeTestBoot : PlayMode.RuntimeTestBoot
    {

        protected override string AdditionalConfig { get; } = null;
        [Test]
        public void InitializationTest()
        {
            Assert.NotNull(Runtime);
            Assert.NotNull(Runtime.Container);
        }
    }
}