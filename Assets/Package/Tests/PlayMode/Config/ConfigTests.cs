using System;
using NUnit.Framework;
using TahaCore.Config;
using TahaCore.Runtime.DI;
using TahaCore.Serialization;
using UnityEngine;
using UnityEngine.Scripting;

namespace TahaCore.Tests.PlayMode.Config
{
    [TestFixture]
    public class ConfigTests : RuntimeTestBoot
    {
        protected override string AdditionalConfig =>
            "[TestConfig]\nIntValue=159\nBoolValue=True\nFloatValue=3.14\nStringValue=TEST_STRING\nLongValue=1234523789\nIntArray = [1,2,3,4,5]";
    
        [Test]
        public void TestConfigSection()
        {
            var testConfigSection = Runtime.Container.Resolve(typeof(TestConfig)) as TestConfig;
            Assert.NotNull(testConfigSection);
            Assert.IsTrue(testConfigSection.SomeInteger == 159);
            Assert.IsTrue(testConfigSection.SomeBoolean == true);
            Assert.IsTrue(Mathf.Approximately(testConfigSection.SomeFloat ,3.14f));
            foreach (var i in testConfigSection.SomeIntArray)
            {
                Debug.Log(i);
            }
            Assert.IsTrue(testConfigSection.SomeString == "TEST_STRING");
        }
    }
    
    [ApplicationRuntimeRegistry(LifetimeType.Singleton)]
    [ConfigSection("TestConfig")]
    public class TestConfig : ConfigSection
    {
        [ConfigProperty("IntValue")] public int SomeInteger { get; set; }
        [ConfigProperty("BoolValue")] public bool SomeBoolean { get; set; }
        [ConfigProperty("FloatValue")] public float SomeFloat { get; set; }
        [ConfigProperty("StringValue")] public string SomeString { get; set; }
        [ConfigProperty("LongValue")] public long SomeLong { get; set; }
        [ConfigProperty("IntArray")] public int[] SomeIntArray { get; set; }
    }
    
    [Preserve]
    public class LongParser : ITypeParser
    {
        public Type TargetType => typeof(long);
        public bool CanBeArrayElement { get; } = true;
        public object Parse(string value)
        {
            return Convert.ToInt64(value);
        }
    }
}