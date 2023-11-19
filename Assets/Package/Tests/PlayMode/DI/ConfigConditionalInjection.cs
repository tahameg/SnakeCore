using NUnit.Framework;
using TahaCore.Runtime.Config;
using TahaCore.Runtime.DI;
using TahaCore.Runtime.DI.ConfigConditions;
using TahaCore.Tests.Runtime;
using UnityEngine;
using VContainer;

namespace TahaCore.Tests.PlayMode.DI
{
    public class ConfigConditionalInjection : RuntimeTestBoot
    {
        protected override string AdditionalConfig  => "[CONFIG_CONDITIONS]\n"+
                                                        "IntCondition=12\n" +
                                                        "BoolCondition=True\n" +
                                                        "ExistCondition=Exists\n";

        private IConfigManager m_configManager;
        
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            m_configManager = Runtime.Container.Resolve(typeof(IConfigManager)) as IConfigManager;
        }
        
        [Test]
        public void IntConditionalInjectionTest()
        {
            Assert.NotNull(Runtime.Container.Resolve(typeof(IntConditionalInjectionPositiveTestType)));
            Assert.Throws(typeof(VContainerException), () => Runtime.Container.Resolve(typeof(IntConditionalInjectionNegativeTestType)));
        }
        
        [Test]
        public void BoolConditionalInjectionTest()
        {
            Debug.Log(m_configManager.GetParam<bool>("CONFIG_CONDITIONS", "BoolCondition"));
            Assert.NotNull(Runtime.Container.Resolve(typeof(BoolConditionalInjectionPositiveTestType)));
            Assert.Throws(typeof(VContainerException), () => Runtime.Container.Resolve(typeof(BoolConditionalInjectionNegativeTestType)));
        }
        
        [Test]
        public void ExistConditionalInjectionTest()
        {
            Assert.NotNull(Runtime.Container.Resolve(typeof(ExistConditionalInjectionPositiveTestType)));
            Assert.Throws(typeof(VContainerException), () => Runtime.Container.Resolve(typeof(ExistConditionalInjectionNegativeTestType)));
        }
        
        [Test]
        public void MultiConditionalInjectionTest()
        {
            Assert.NotNull(Runtime.Container.Resolve(typeof(MultiConditionalPositiveTestType)));
            Assert.Throws(typeof(VContainerException), () => Runtime.Container.Resolve(typeof(MultiConditionalNegativeTestType)));
        }
        
    }
    
    [ApplicationRuntimeRegistry(LifetimeType.Singleton)]
    [IntValueConfigCondition("CONFIG_CONDITIONS", "IntCondition", 12, NumericValueCompareType.Equal)]
    public class IntConditionalInjectionPositiveTestType
    {
        
    }
    
    [ApplicationRuntimeRegistry(LifetimeType.Singleton)]
    [IntValueConfigCondition("CONFIG_CONDITIONS", "IntCondition", 11, NumericValueCompareType.Equal)]
    public class IntConditionalInjectionNegativeTestType
    {
        
    }
    
    [ApplicationRuntimeRegistry(LifetimeType.Singleton)]
    [BoolConfigCondition("CONFIG_CONDITIONS", "BoolCondition", true)]
    public class BoolConditionalInjectionPositiveTestType
    {
        
    }
    
    [ApplicationRuntimeRegistry(LifetimeType.Singleton)]
    [BoolConfigCondition("CONFIG_CONDITIONS", "BoolCondition", false)]
    public class BoolConditionalInjectionNegativeTestType
    {
        
    }
    
    [ApplicationRuntimeRegistry(LifetimeType.Singleton)]
    [KeyExistenceConfigCondition("CONFIG_CONDITIONS", "ExistCondition", ExistenceType.Exists)]
    public class ExistConditionalInjectionPositiveTestType
    {
        
    }
    
    [ApplicationRuntimeRegistry(LifetimeType.Singleton)]
    [KeyExistenceConfigCondition("CONFIG_CONDITIONS", "ExistCondition_2", ExistenceType.Exists)]
    public class ExistConditionalInjectionNegativeTestType
    {
        
    }

    [ApplicationRuntimeRegistry(LifetimeType.Singleton)]
    [KeyExistenceConfigCondition("CONFIG_CONDITIONS", "ExistCondition", ExistenceType.Exists)]
    [IntValueConfigCondition("CONFIG_CONDITIONS", "IntCondition", 13, NumericValueCompareType.LessThanOrEqual)]
    [BoolConfigCondition("CONFIG_CONDITIONS", "BoolCondition", true)]
    public class MultiConditionalPositiveTestType
    {
        
    }
    
    [ApplicationRuntimeRegistry(LifetimeType.Singleton)]
    [KeyExistenceConfigCondition("CONFIG_CONDITIONS", "ExistCondition", ExistenceType.Exists)]
    [IntValueConfigCondition("CONFIG_CONDITIONS", "IntCondition", 11, NumericValueCompareType.LessThanOrEqual)]
    [BoolConfigCondition("CONFIG_CONDITIONS", "BoolCondition", false)]
    public class MultiConditionalNegativeTestType
    {
        
    }
    
    
}