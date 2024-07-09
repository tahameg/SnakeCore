namespace SnakeCore.Serialization.JsonSerialization
{
    /// <summary>
    /// Common interface that provide json serialization and deserialization.
    /// </summary>
    public interface IJsonSerializer : ISerializer
    {
        object Deserialize(string serialized);
    }
}