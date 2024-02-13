using System.IO;

namespace TahaCore.Serialization
{
    public interface IDeserializer
    {
        T Deserialize<T>(string serialized);
        
        object Deserialize(string serialized);
    }
}