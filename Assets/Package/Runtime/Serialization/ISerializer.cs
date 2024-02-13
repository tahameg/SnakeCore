namespace TahaCore.Serialization
{
    public interface ISerializer
    {
        string Serialize<T>(T obj);
        
        string Serialize(object obj);
    }
}