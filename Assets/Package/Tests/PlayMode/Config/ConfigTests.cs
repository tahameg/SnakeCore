using System;
using NUnit.Framework;
using TahaCore.Config;
using TahaCore.DI;
using TahaCore.Serialization;
using TahaCore.Serialization.JsonSerialization;
using TahaCore.Serialization.TypeParsers;
using UnityEngine;
using UnityEngine.Scripting;

namespace TahaCore.Tests.PlayMode.Config
{
    [TestFixture]
    public class ConfigTests : RuntimeTestBoot
    {
        protected override string AdditionalConfig =>
            "[TestConfig]\nIntValue=159\nBoolValue=True\nFloatValue=3.14\nStringValue=TEST_STRING\nLongValue=1234523789\nIntArray=[1,2,3,4,5]";
        private TestConfig m_testConfig;
        
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            m_testConfig = Runtime.Container.Resolve(typeof(TestConfig)) as TestConfig;
        }

        [Test]
        public void ConfigSectionPrimitivesTest()
        {
            Assert.NotNull(m_testConfig);
            Assert.IsTrue(m_testConfig.SomeInteger == 159);
            Assert.IsTrue(m_testConfig.SomeBoolean == true);
            Assert.IsTrue(Mathf.Approximately(m_testConfig.SomeFloat ,3.14f));
            Assert.IsTrue(m_testConfig.SomeString == "TEST_STRING");
        }

        [Test]
        public void ConfigSectionArrayParsingTest()
        {
            Assert.NotNull(m_testConfig);
            for(int i = 0; i < m_testConfig.SomeIntArray.Length; i++)
                Assert.IsTrue(m_testConfig.SomeIntArray[i] == i + 1);
        }
        
        [Test]
        public void UseParserTest()
        {
            Assert.NotNull(m_testConfig);
            Assert.IsTrue(m_testConfig.SomeLong == 1234523789);
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
        
        [ParseWith(typeof(LongParser))]
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