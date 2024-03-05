namespace TahaCore.Serialization
{
    /// <summary>
    /// Common interface for serialization.
    /// </summary>
    public interface ISerializer
    {
        string Serialize<T>(T obj);
        
        string Serialize(object obj);
    }
}