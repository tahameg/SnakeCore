using System;

namespace SnakeCore.Serialization
{
    /// <summary>
    /// Created to provide functionality of System.Serializable specific to TahaCore
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class SerializableTypeAttribute : Attribute
    {
    }
}