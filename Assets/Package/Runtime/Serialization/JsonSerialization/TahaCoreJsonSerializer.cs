using System;
using Unity.Plastic.Newtonsoft.Json;

namespace TahaCore.Serialization.JsonSerialization
{
    public class TahaCoreJsonSerializer : IJsonSerializer
    {
        private TahaCoreJsonConverter m_converter;
        public TahaCoreJsonSerializer()
        {
            m_converter = new TahaCoreJsonConverter(new SerializationContext(),
                new ParsingProvider(new JsonTypeParserLocator()));
        }
        public T Deserialize<T>(string serialized)
        {
            return JsonConvert.DeserializeObject<T>(serialized, m_converter);
        }

        public object Deserialize(string serialized)
        {
            return JsonConvert.DeserializeObject(serialized, typeof(object), m_converter);
        }

        public string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, m_converter);
        }

        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, m_converter);
        }
    }
}