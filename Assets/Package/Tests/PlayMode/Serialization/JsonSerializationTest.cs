using System.Collections.Generic;
using Newtonsoft.Json;
using NUnit.Framework;
using SnakeCore.Serialization;
using SnakeCore.Serialization.JsonSerialization;
using UnityEngine;

namespace SnakeCore.Tests.PlayMode.Serialization
{
    public class JsonSerializationTest : RuntimeTestBoot
    {
        protected override string AdditionalConfig { get; } = "";

        private IJsonSerializer m_jsonSerializer;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            m_jsonSerializer = Runtime.Container.Resolve(typeof(IJsonSerializer)) as IJsonSerializer;
        }

        [Test]
        public void DeserializationTest()
        {
            var result = m_jsonSerializer.Deserialize(TEST_JSON_1);
            Assert.IsInstanceOf<SerializableTestClass>(result);
            SerializableTestClass serializableTestClass = result as SerializableTestClass;
            Assert.NotNull(serializableTestClass?.TestObject);
            Assert.AreEqual(5, serializableTestClass.TestInt);
            Assert.AreEqual(3.0f, serializableTestClass.TestObject.TestFloat);
        }

        [Test]
        public void DeserializationPolymorphicTest()
        {
            SerializableTestClass testSample =
                new SerializableTestClass()
                {
                    TestInt = 5,
                    TestObject = new ExtendedSerializableInnerClass()
                    {
                      TestFloat = 3f,
                      SerializableVector3 = new Vector3(0, 1, 2),
                      ExtendedTestFloat = 4f,
                    },
                    TestList = new List<int> {1, 2, 3, 4, 5},
                    
                };
            var serialized = m_jsonSerializer.Serialize(testSample);
            Debug.Log(serialized);
            
            var result = m_jsonSerializer.Deserialize(serialized);
            SerializableTestClass serializableTestClass = result as SerializableTestClass;
            Assert.NotNull(serializableTestClass);

            ExtendedSerializableInnerClass innerClass = serializableTestClass.TestObject as ExtendedSerializableInnerClass;
            Assert.NotNull(innerClass);
            Assert.AreEqual(3.0f, innerClass.TestFloat);
            Assert.AreEqual(5, serializableTestClass.TestList.Count);
            
            int i = 0;
            foreach (var item in serializableTestClass.TestList)
            {
                Assert.AreEqual(i + 1, item);
                i++;    
            }
        }
        
        [Test]
        public void SerializationTest()
        {
            SerializableTestClass serializableTestClass =
                new SerializableTestClass()
                {
                    TestInt = 5,
                    TestObject = new SerializableInnerClass()
                    {
                        TestFloat = 3.0f,
                        SerializableVector3 = new Vector3(0, 1, 2)
                    },
                    TestList = new List<int> {1, 2, 3, 4, 5},
                };
            
            string serialized = m_jsonSerializer.Serialize(serializableTestClass);
            Debug.Log(serialized);
            Assert.IsTrue(serialized.Contains("\"testInt\":5"));
        }
        
        
        
        [Test]
        public void NonSerializableTypeTest()
        {
            NonSerializableTestClass nonSerializableTestClass = new NonSerializableTestClass();
            Assert.Catch<JsonException>(() => m_jsonSerializer.Serialize(nonSerializableTestClass));
        }
        
        [Test]
        public void DeserializeRootArray()
        {
            SerializableTestClass testSample =
                new SerializableTestClass()
                {
                    TestInt = 5,
                    TestObject = new ExtendedSerializableInnerClass()
                    {
                        TestFloat = 3f,
                        SerializableVector3 = new Vector3(0, 1, 2),
                        ExtendedTestFloat = 4f,
                    },
                    TestList = new List<int> {1, 2, 3, 4, 5},
                    
                };
            
            List<SerializableTestClass> list = new List<SerializableTestClass> {testSample, testSample, testSample};
            string serialized = m_jsonSerializer.Serialize(list);

            var result = m_jsonSerializer.Deserialize<List<SerializableTestClass>>(serialized);
            Assert.NotNull(result);
            Assert.AreEqual(3, result.Count);
            foreach (var item in result)
            {
                Assert.AreEqual(5, item.TestInt);
                Assert.AreEqual(3.0f, item.TestObject.TestFloat);
            }
        }
        
        private const string TEST_JSON_1 = 
            "{" +
            "   \"$type\":\"SnakeCore.Tests.PlayMode.Serialization.SerializableTestClass\"," + 
            "   \"testInt\":5," +
            "   \"testObject\": {" +
            "       \"$type\":\"SnakeCore.Tests.PlayMode.Serialization.SerializableInnerClass\"," + 
            "       \"testFloat\":3.0," +
            "       \"serializableVector3\": \"(0, 1, 2)\"" +
            "   }" +
            "}";
        
        private const string TEST_JSON_2 = 
            "{" +
            "   \"$type\":\"TahaCore.Tests.PlayMode.Serialization.ExtendedSerializableTestClass\"," + 
            "   \"testInt\":5," +
            "   \"extendedTestObject\": {" +
            "       \"$type\":\"TahaCore.Tests.PlayMode.Serialization.ExtendedSerializableInnerClass\"," + 
            "       \"testFloat\":3.0," +
            "       \"serializableVector3\": \"(0, 1, 2)\"," +
            "       \"extendedTestFloat\":3.0" +
            "   }" +
            "}";
    }

    [SerializableType]
    public class SerializableTestClass
    {
        [SerializableProperty("testInt")] public int TestInt { get; set; }

        [SerializableProperty("testList")] public List<int> TestList { get; set; }
        [SerializableProperty("testObject")] public SerializableInnerClass TestObject { get; set; }

    }

    [SerializableType]
    public class SerializableInnerClass
    {
        [SerializableProperty("testFloat")] public float TestFloat { get; set; } = 6.0f;

        [SerializableProperty("serializableVector3")]
        public Vector3 SerializableVector3 { get; set; }

    }

    public class ExtendedSerializableTestClass : SerializableTestClass
    {
        [SerializableProperty("extendedTestObject")] public SerializableInnerClass ExtendedTestObject { get; set; }
    }
    
    public class ExtendedSerializableInnerClass : SerializableInnerClass
    {
        [SerializableProperty("extendedTestFloat")] public float ExtendedTestFloat { get; set; } = 7.0f;
    }
    public class NonSerializableTestClass
    {
        public int TestInt { get; set; } = 10;
    }
    
}
