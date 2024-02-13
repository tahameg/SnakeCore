using System;

namespace TahaCore.Serialization
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class SerializableClassAttribute : Attribute
    {
    }
}