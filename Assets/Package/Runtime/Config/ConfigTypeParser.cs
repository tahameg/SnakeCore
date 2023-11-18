using System;
using TahaCore.Config.Types;
using TahaCore.DI;
using UnityEngine;
using DateTime = TahaCore.Config.Types.DateTime;
using Double = TahaCore.Config.Types.Double;
using Quaternion = TahaCore.Config.Types.Quaternion;
using Single = TahaCore.Config.Types.Single;
using Vector2 = TahaCore.Config.Types.Vector2;
using Vector3 = TahaCore.Config.Types.Vector3;
using Guid = TahaCore.Config.Types.Guid;

namespace TahaCore.Config
{
    [ApplicationRuntimeRegistry(LifetimeType.Instanced, typeof(IConfigTypeParser))]
    public class ConfigTypeParser : IConfigTypeParser
    {
        /// <summary>
        /// Supported types: <br/>
        /// int,
        /// float,
        /// double,
        /// bool,
        /// int[],
        /// float[],
        /// double[],
        /// Vector2,
        /// Vector3,
        /// Vector4,
        /// Quaternion,
        /// string,
        /// DateTime,
        /// Guid
        /// </summary>
        /// <param name="value">String value to parse.</param>
        /// <typeparam name="T">Target type.</typeparam>
        /// <returns>Parsed value.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the given type is not supported.</exception>
        public T Parse<T>(string value)
        {  
           Type type = typeof(T);
           if (type == typeof(int))
           {
               return (T)Convert.ChangeType(new Integer(value).Value, type);
           }
           
           if (type == typeof(float))
           {
               return (T)Convert.ChangeType(new Single(value).Value, type);
           }
           
           if (type == typeof(double))
           {
               return (T)Convert.ChangeType(new Double(value).Value, type);
           }
           
           if (type == typeof(bool))
           {
               return (T)Convert.ChangeType(new Bool(value).Value, type);
           }

           if (type == typeof(int[]))
           {
               return (T)Convert.ChangeType(new IntegerArray(value).Value, type);
           }

           if (type == typeof(float[]))
           {
               return (T)Convert.ChangeType(new SingleArray(value).Value, type);
           }

           if (type == typeof(double[]))
           {
               return (T)Convert.ChangeType(new DoubleArray(value).Value, type);
           }

           if (type == typeof(UnityEngine.Vector2))
           {
               return (T)Convert.ChangeType(new Vector2(value).Value, type);
           }

           if (type == typeof(UnityEngine.Vector3))
           {
               return (T)Convert.ChangeType(new Vector3(value).Value, type);
           }

           if (type == typeof(UnityEngine.Quaternion))
           {
               return (T)Convert.ChangeType(new Quaternion(value).Value, type);
           }

           if (type == typeof(Vector2Int))
           {
               return (T)Convert.ChangeType(new IntVector2(value).Value, type);
           }

           if (type == typeof(Vector3Int))
           {
               return (T)Convert.ChangeType(new IntVector3(value).Value, type);
           }
           
           if(type == typeof(string))
           {
               return (T)Convert.ChangeType(value.Trim(), type);
           }

           if (type == typeof(System.DateTime))
           {
               return (T)Convert.ChangeType(new DateTime(value).Value, type);
           }
           
           if(type == typeof(System.Guid))
           {
               return (T)Convert.ChangeType(new Guid(value).Value, type);
           }
           
           throw new InvalidOperationException($"Type {type} is not supported");
        }
    }
}