using System;

namespace TahaCore.Serialization
{
    public interface ITypeParsingProvider
    {
        object Parse(Type targetType, string value);
        T Parse<T>(string value);
    }
}