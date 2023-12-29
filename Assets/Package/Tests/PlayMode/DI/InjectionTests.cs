using System;
using NUnit.Framework;
using TahaCore.Config;
using TahaCore.Runtime.DI;
using TahaCore.Serialization;
using UnityEngine;
using UnityEngine.Scripting;

namespace TahaCore.Tests.PlayMode.DI
{
    [TestFixture]
    public class InjectionTests : RuntimeTestBoot
    {
        protected override string AdditionalConfig => "[TEST_STUFF]\nTEST_STUFF=TEST_STUFF";
        private IFirstTestInterface m_firstTestInterface;
        private ISecondTestInterface m_secondTestInterface;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            m_firstTestInterface = Runtime.Container.Resolve(typeof(IFirstTestInterface)) as IFirstTestInterface;
            m_secondTestInterface = Runtime.Container.Resolve(typeof(ISecondTestInterface)) as ISecondTestInterface;
        }

        [Test]
        public void InjectRegistrationWithInterface()
        {
            Assert.NotNull(m_secondTestInterface);
        }

        [Test]
        public void InjectRegistrationWithMultipleInterfaces()
        {
            Assert.AreSame(m_secondTestInterface, m_firstTestInterface);
        }

        [Test]
        public void SelfInjectionTest()
        {
            Assert.NotNull(Runtime.Container.Resolve(typeof(SelfInjectionTestType)));
        }
    }

    public interface IFirstTestInterface
    {
        string TestString1 { get; }
    }
    
    public interface ISecondTestInterface
    {
        string TestString2 { get; }
    }
    
    [ApplicationRuntimeRegistry(LifetimeType.Singleton, typeof(IFirstTestInterface), typeof(ISecondTestInterface))]
    public class InjectionTestType : IFirstTestInterface, ISecondTestInterface
    {
        public string TestString1 => "TestString1";
        public string TestString2 => "TestString2";
    }

    [ApplicationRuntimeRegistry(LifetimeType.Singleton)]
    public class SelfInjectionTestType
    {
    }
}