using System;
using Unity.Plastic.Newtonsoft.Json;

namespace TahaCore.Serialization.JsonSerialization
{
    public class TahaCoreJsonSerializer : IJsonSerializer
    {
        public T Deserialize<T>(string serialized)
        {
            return JsonConvert.DeserializeObject<T>(serialized);
        }

        public object Deserialize(string serialized)
        {
            return JsonConvert.DeserializeObject(serialized);
        }

        public T Deserialize<T>(System.IO.Stream stream)
        {
            throw new NotImplementedException();
        }

        public object Deserialize(System.IO.Stream stream)
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