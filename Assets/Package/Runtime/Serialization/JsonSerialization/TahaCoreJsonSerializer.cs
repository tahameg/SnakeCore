using System;
using TahaCore.DI;
using Unity.Plastic.Newtonsoft.Json;

namespace TahaCore.Serialization.JsonSerialization
{
    [ApplicationRuntimeRegistry(LifetimeType.Singleton, typeof(IJsonSerializer))]
    public class TahaCoreJsonSerializer : IJsonSerializer
    {
        public T Deserialize<T>(string serialized)
        {
            return JsonConvert.DeserializeObject<T>(serialized);
        }

        public object Deserialize(string serialized)
        {
            throw new NotImplementedException();
        }

        public string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}