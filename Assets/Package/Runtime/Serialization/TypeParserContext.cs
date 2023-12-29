using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TahaCore.Reflection;
using TahaCore.Runtime.DI;
using UnityEngine;

namespace TahaCore.Serialization
{
   internal class TypeParserContext
   {
      private readonly HashSet<ITypeParser> m_deserializers = new ();
      private readonly ArrayTypeParser m_arrayTypeParser;
      
      internal TypeParserContext()
      {
         var deserializerTypes = TypeUtility.GetTypes(ShouldBeRegistered);
         foreach (var deserializerType in deserializerTypes)
         {
            var deserializer = Activator.CreateInstance(deserializerType, true) as ITypeParser;
            if(deserializer == null) continue;
            m_deserializers.Add(deserializer);
            if (deserializer.CanBeArrayElement)
            {
               var arrayDeserializer = new ArrayTypeParser(deserializer);
               m_deserializers.Add(arrayDeserializer);
            }
         }
      }

      internal ITypeParser GetParserForType(Type targetType)
      {
         if(targetType == null) throw new ArgumentNullException(nameof(targetType));
         var found = m_deserializers.First(deserializer => deserializer.TargetType == targetType);
         if (found == null)
         {
            throw new ArgumentException($"No deserializer found for type {targetType.Name}.");
         }

         return found;
      }
      
      internal ITypeParser GetParser<T>() where T : ITypeParser
      {
         var found = m_deserializers.First(deserializer => deserializer.GetType() == typeof(T));
         return found;
      }
      
      private bool ShouldBeRegistered(Type type)
      {
         if (!typeof(ITypeParser).IsAssignableFrom(type) || type.IsAbstract) return false;
         if(!HasParameterlessConstructor(type)) return false;
         return true;
      }
      
      private bool HasParameterlessConstructor(Type type)
      {
         ConstructorInfo constructor 
            = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, 
               null, Type.EmptyTypes, null);

         return constructor != null;
      }
   }
}