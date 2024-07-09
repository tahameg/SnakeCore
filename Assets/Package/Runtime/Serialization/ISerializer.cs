using System;
using System.IO;
using Cysharp.Threading.Tasks;

namespace SnakeCore.Serialization
{
    public interface ISerializer
    {
        T Deserialize<T>(string serialized);
        
        object Deserialize(string serialized, Type targetType);  
        string Serialize<T>(T obj);
        string Serialize(object obj);
        
        UniTask<T> DeserializeAsync<T>(Stream stream);
        
        UniTask<object> DeserializeAsync(Stream stream);
    }
}