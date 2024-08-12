using System;
using System.IO;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;

namespace SnakeCore.Serialization.JsonSerialization
{
    /// <summary>
    /// Serializes and deserializes objects to and from json format using Newtonsoft.Json.
    /// </summary>
    public class NewtonsoftJsonSerializer : IJsonSerializer
    {
        JsonConverter m_jsonConverter;
        JsonSerializerSettings m_jsonSerializerSettings;
        
        /// <summary>
        /// Creates a new instance of the serializer.
        /// </summary>
        /// <param name="serializationContext">Serialization context to use.</param>
        public NewtonsoftJsonSerializer(ISerializationContext serializationContext)
        {
            m_jsonConverter = new SnakeCoreJsonConverter(serializationContext); 
        }

        /// <summary>
        /// Deserializes the given string to an object of type T.
        /// </summary>
        public T Deserialize<T>(string serialized)
        {
            return JsonConvert.DeserializeObject<T>(serialized, m_jsonConverter);
        }

        /// <summary>
        /// Deserializes the given string to an object of the given type.
        /// </summary>
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
        
        /// <summary>
        /// Serializes the given object to a json string.
        /// </summary>
        public string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, m_jsonConverter);
        }

        /// <summary>
        /// Serializes the given object to a json string.
        /// </summary>
        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, m_jsonConverter);
        }
        
        /// <summary>
        /// Deserializes the given stream to an object of type T.
        /// </summary>
        public async UniTask<T> DeserializeAsync<T>(Stream stream)
        {
            using StreamReader reader = new StreamReader(stream);
            
            string serialized = await reader.ReadToEndAsync();
            return Deserialize<T>(serialized);
        }
    
        /// <summary>
        /// Deserializes the given stream to an object.
        /// </summary>
        public async UniTask<object> DeserializeAsync(Stream stream)
        {
            using StreamReader reader = new StreamReader(stream);
            
            string serialized = await reader.ReadToEndAsync();
            return UniTask.FromResult(Deserialize(serialized));
        }
    }
}