using System;
using System.IO;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;

namespace SnakeCore.Serialization.JsonSerialization
{
    /// <summary>
    /// A temporary solution to use until a more comprehensive and type-based serializer is implemented.
    /// </summary>
    public class NewtonsoftJsonSerializer : IJsonSerializer
    {
        JsonConverter m_jsonConverter;
        JsonSerializerSettings m_jsonSerializerSettings;
        public NewtonsoftJsonSerializer(ISerializationContext serializationContext)
        {
            m_jsonConverter = new TahaCoreJsonConverter(serializationContext); 
        }

        public T Deserialize<T>(string serialized)
        {
            return JsonConvert.DeserializeObject<T>(serialized, m_jsonConverter);
        }

        public object Deserialize(string serialized, Type targetType)
        {
            return JsonConvert.DeserializeObject(serialized, targetType, m_jsonConverter);
        }

        /// <summary>
        /// Tries to deserialize the given string to an object.
        /// The strategy is using the $type property to determine the type of the object.
        /// </summary>
        public object Deserialize(string serialized)
        {
            return JsonConvert.DeserializeObject(serialized, typeof(object), m_jsonConverter);
        }

        public string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, m_jsonConverter);
        }

        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, m_jsonConverter);
        }

        public async UniTask<T> DeserializeAsync<T>(Stream stream)
        {
            using StreamReader reader = new StreamReader(stream);
            
            string serialized = await reader.ReadToEndAsync();
            return Deserialize<T>(serialized);
        }

        public async UniTask<object> DeserializeAsync(Stream stream)
        {
            using StreamReader reader = new StreamReader(stream);
            
            string serialized = await reader.ReadToEndAsync();
            return UniTask.FromResult(Deserialize(serialized));
        }
    }
}