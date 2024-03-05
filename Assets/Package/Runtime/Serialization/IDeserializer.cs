
namespace TahaCore.Serialization
{
    /// <summary>
    /// Common interface for deserialization.
    /// </summary>
    public interface IDeserializer
    {
        /// <summary>
        /// Deserializes to given type.
        /// </summary>
        T Deserialize<T>(string serialized);
        
        /// <summary>
        /// Deserializes to object.
        /// </summary>
        object Deserialize(string serialized);
    }
}