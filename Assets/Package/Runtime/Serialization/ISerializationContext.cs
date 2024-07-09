using System;

namespace SnakeCore.Serialization
{
    /// <summary>
    /// Defines an application-wise context for serialization. The serialization operations are
    /// performed based on the serialization context. The types that are not known by the context will
    /// not be serialized. This creates a security layer for the serialization.
    /// </summary>
    public interface ISerializationContext
    {
        /// <summary>
        /// Gets the serialization info for the given type.
        /// </summary>
        bool TryGetSerializationInfo(Type type, out SerializationInfo serializationInfo);
        
        bool IsSerializable(Type type);
    }
}