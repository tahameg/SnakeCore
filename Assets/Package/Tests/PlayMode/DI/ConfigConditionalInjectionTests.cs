using NUnit.Framework;
using TahaCore.DI;
using TahaCore.DI.ConfigConditions;
using VContainer;

namespace TahaCore.Tests.PlayMode.DI
{
    [TestFixture]
    public class ConfigConditionalInjectionTests : RuntimeTestBoot
    {
        protected override string AdditionalConfig  => "[CONFIG_CONDITIONS]\n"+
                                                        "IntCondition=12\n" +
                                                        "BoolCondition=True\n" +
                                                        "ExistCondition=Exists\n";
        [Test]
        public void IntConditionalInjectionTest()
        {
            Assert.NotNull(Runtime.Container.Resolve(typeof(IntConditionalInjectionPositiveTestType)));
            Assert.Throws(typeof(VContainerException), () => Runtime.Container.Resolve(typeof(IntConditionalInjectionNegativeTestType)));
        }
        
        [Test]
        public void BoolConditionalInjectionTest()
        {
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
    [KeyExistenceConfigCondition("CONFIG_CONDITIONS", "ExistCondition", ExistenceCompareType.Exists)]
    public class ExistConditionalInjectionPositiveTestType
    {
        
    }
    
    [ApplicationRuntimeRegistry(LifetimeType.Singleton)]
    [KeyExistenceConfigCondition("CONFIG_CONDITIONS", "ExistCondition_2", ExistenceCompareType.Exists)]
    public class ExistConditionalInjectionNegativeTestType
    {
        
    }

    [ApplicationRuntimeRegistry(LifetimeType.Singleton)]
    [KeyExistenceConfigCondition("CONFIG_CONDITIONS", "ExistCondition", ExistenceCompareType.Exists)]
    [IntValueConfigCondition("CONFIG_CONDITIONS", "IntCondition", 13, NumericValueCompareType.LessThanOrEqual)]
    [BoolConfigCondition("CONFIG_CONDITIONS", "BoolCondition", true)]
    public class MultiConditionalPositiveTestType
    {
        
    }
    
    [ApplicationRuntimeRegistry(LifetimeType.Singleton)]
    [KeyExistenceConfigCondition("CONFIG_CONDITIONS", "ExistCondition", ExistenceCompareType.Exists)]
    [IntValueConfigCondition("CONFIG_CONDITIONS", "IntCondition", 11, NumericValueCompareType.LessThanOrEqual)]
    [BoolConfigCondition("CONFIG_CONDITIONS", "BoolCondition", false)]
    public class MultiConditionalNegativeTestType
    {
    }
}