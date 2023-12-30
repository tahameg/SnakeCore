// ==============================License==================================
// MIT License
// Author: Taha Mert Gökdemir
// =======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TahaCore.Reflection;

namespace TahaCore.Serialization
{
   /// <summary>
   /// Automatically Caches and provides <see cref="ITypeParser"/>s that are defined TypeParserContextRegistryAttribute.
   /// The registration happens in the constructor.
   /// Note that if there are multiple <see cref="ITypeParser"/>s that can parse the same type, the first one is used.
   /// </summary>
   internal class AutoBoundTypeParserContext : ITypeParserContext
   {
      private readonly HashSet<ITypeParser> m_deserializers = new ();
      private readonly ArrayTypeParser m_arrayTypeParser;
      
      /// <summary>
      /// Creates a new TypeParserContext and registers all <see cref="ITypeParser"/>s that are defined in all assemblies.
      /// </summary>
      internal AutoBoundTypeParserContext()
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

      /// <summary>
      /// Get parser for the given type.
      /// </summary>
      /// <param name="targetType">Type to get parser for.</param>
      /// <returns>Parser for the given type.</returns>
      /// <exception cref="ArgumentNullException">If the given type is null.</exception>
      /// <exception cref="ArgumentException">If no parser is found for the given type.</exception>
      public ITypeParser GetParserForType(Type targetType)
      {
         if(targetType == null) throw new ArgumentNullException(nameof(targetType));
         var found = m_deserializers.FirstOrDefault(deserializer => deserializer.TargetType == targetType);
         if (found == null)
         {
            throw new ArgumentException($"No deserializer found for type {targetType.Name}.");
         }

         return found;
      }
      
      /// <summary>
      /// Get parser of type T.
      /// </summary>
      /// <typeparam name="T">Type of parser to get.</typeparam>
      /// <returns>Parser of type T. Null if no parser was found.</returns>
      public ITypeParser GetParser<T>() where T : ITypeParser
      {
         var found = m_deserializers.FirstOrDefault(deserializer => deserializer.GetType() == typeof(T));
         return found;
      }
      
      //Returns if the type should be registered.
      private bool ShouldBeRegistered(Type type)
      {
         if (!type.IsDefined(typeof(TypeParserContextRegistryAttribute), false)) return false;
         if (!typeof(ITypeParser).IsAssignableFrom(type) || type.IsAbstract) return false;
         return HasParameterlessConstructor(type);
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