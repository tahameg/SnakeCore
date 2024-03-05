using NUnit.Framework;
using TahaCore.Scene;

namespace TahaCore.Tests.PlayMode.SceneEvents
{
    public class SceneEventTests : RuntimeTestBoot
    {
        ISceneEventHistory m_sceneEventHistory;
        ISceneEventProvider m_sceneEventProvider;
        protected override string AdditionalConfig { get; } = "";
        
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            m_sceneEventHistory = Runtime.Container.Resolve(typeof(ISceneEventHistory)) as ISceneEventHistory;
            m_sceneEventProvider = Runtime.Container.Resolve(typeof(ISceneEventProvider)) as ISceneEventProvider;
        }
        
        [Test]
        public void SceneEventPublishingTest()
        {
            TestSubscriber testSubscriber = new TestSubscriber(m_sceneEventProvider);
            TestSceneEvent testSceneEvent = new TestSceneEvent();
            m_sceneEventHistory.AddSceneEvent(testSceneEvent);
            Assert.AreEqual(testSceneEvent.TestInt, testSubscriber.TestInt);
        }
        
        [Test]
        public void SceneEventPublishingNonGenericTest()
        {
            TestSubscriber testSubscriber = new TestSubscriber(m_sceneEventProvider);
            SceneEvent testSceneEvent = new TestSceneEvent();
            m_sceneEventHistory.AddSceneEvent(testSceneEvent);
            Assert.AreEqual(((TestSceneEvent)testSceneEvent).TestInt, testSubscriber.TestInt);
        }
    }

    public class TestSceneEvent : SceneEvent
    {
        public int TestInt => 14;
    }

    public class TestSubscriber
    {
        public int TestInt { get; private set; }
        
        public TestSubscriber(ISceneEventProvider sceneEventProvider)
        {
            sceneEventProvider.Subscribe<TestSceneEvent>(OnTestSceneEvent);
        }

        private void OnTestSceneEvent(SceneEvent sceneEvent)
        {
            if(sceneEvent is not TestSceneEvent testSceneEvent) return;
            TestInt = testSceneEvent.TestInt;
        }
    }
}
