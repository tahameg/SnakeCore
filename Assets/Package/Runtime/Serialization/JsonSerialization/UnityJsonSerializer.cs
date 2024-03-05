using System;
using TahaCore.DI;
using UnityEngine;



namespace TahaCore.Serialization.JsonSerialization
{
    /// <summary>
    /// A temporary solution to use until a more comprehensive and type-based serializer is implemented.
    /// </summary>
    [ApplicationRuntimeRegistry(LifetimeType.Singleton, typeof(IJsonSerializer))]
    public class UnityJsonSerializer : IJsonSerializer
    {
        public T Deserialize<T>(string serialized)
        {
            return JsonUtility.FromJson<T>(serialized);
        }

        /// <summary>
        /// Deserializing to unknown type is not supported yet.
        /// </summary>
        public object Deserialize(string serialized)
        {
            throw new NotImplementedException("Deserializing to unknown type is not supported yet.");
        }

        public string Serialize<T>(T obj)
        {
            return JsonUtility.ToJson(obj);
        }

        public string Serialize(object obj)
        {
            return JsonUtility.ToJson(obj);
        }
    }
}