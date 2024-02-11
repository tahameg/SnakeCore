using System.IO;

namespace TahaCore.Serialization
{
    public interface IDeserializer
    {
        T Deserialize<T>(string serialized);
        
        object Deserialize(string serialized);
        
        T Deserialize<T>(Stream stream);
        
        object Deserialize(Stream stream);
    }
}