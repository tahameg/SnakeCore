using System;

namespace TahaCore.Serialization
{
    /// <summary>
    /// Created to provide functionality of System.Serializable specific to TahaCore
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class SerializableClassAttribute : Attribute
    {
    }
}