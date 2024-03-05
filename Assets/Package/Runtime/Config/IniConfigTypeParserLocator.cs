// ==============================License==================================
// MIT License
// Author: Taha Mert Gökdemir
// =======================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TahaCore.Config.TypeParsers;
using TahaCore.DI;
using TahaCore.Reflection;
using TahaCore.Serialization;
using TahaCore.Serialization.TypeParsers;

namespace TahaCore.Config
{
   /// <summary>
   /// Automatically Caches and provides <see cref="ITypeParser"/>s that are defined ConfigTypeParserAttribute.
   /// The registration happens in the constructor.
   /// Note that if there are multiple <see cref="ITypeParser"/>s that can deserialize the same type, the first one is used.
   /// </summary>
   [ApplicationRuntimeRegistry(LifetimeType.Singleton)]
   internal class IniConfigTypeParserLocator : TypeParserLocator
   {
      private readonly IDictionary<Type, ArrayTypeParser> m_elementTypeToArrayParser = new Dictionary<Type, ArrayTypeParser>();

      /// <summary>
      /// Creates a new TypeParserContext and registers all <see cref="ITypeParser"/>s that are defined in all assemblies.
      /// </summary>
      internal IniConfigTypeParserLocator()
      {
         var deserializerTypes = TypeUtility.GetTypes(ShouldBeRegistered);
         foreach (var deserializerType in deserializerTypes)
         {
            var deserializer = Activator.CreateInstance(deserializerType, true) as ITypeParser;
            if(deserializer == null) continue;
            if (!TryRegisterParser(deserializer))
            {
               TahaCoreApplicationRuntime.LogError(
                  $"TypeParser for target type {deserializer.TargetType} is already registered. " +
                  $"Ignoring {deserializerType.Name}.");
            }
            
         }
      }

      //Returns if the type should be registered.
      private bool ShouldBeRegistered(Type type)
      {
         if (!type.IsDefined(typeof(ConfigTypeParserAttribute), false)) return false;
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

      public override ITypeParser GetParser<T>()
      {
         var found = base.GetParser<T>();
         if (found != null) return found;
         if (!typeof(ArrayTypeParser).IsAssignableFrom(typeof(T))) return null;
         var (_, value) 
            = m_elementTypeToArrayParser.FirstOrDefault(keyValue => keyValue.Value.TargetType == typeof(T));
         if(value != null) return value;
         return null;
      }
      
      public override ITypeParser GetParserForType(Type targetType)
      {
         if(targetType == null) throw new ArgumentNullException(nameof(targetType));
         var found = base.GetParserForType(targetType);
         if (found != null) return found;
         if (targetType.IsArray)
         {
            return FindOrCreateArrayTypeParser(targetType);
         }

         return null;
      }


      private ITypeParser FindOrCreateArrayTypeParser(Type arrayType)
      {
         if (!arrayType.IsArray) return null;
         var elementType = arrayType.GetElementType();
         if (elementType == null) return null;
         if (!m_elementTypeToArrayParser.TryGetValue(elementType, out var parser))
         {
            parser = new ArrayTypeParser(elementType);
            m_elementTypeToArrayParser[elementType] = parser;
         }
         
         return parser;
      }
   }
}