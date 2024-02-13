using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TahaCore.Config;
using TahaCore.Config.TypeParsers;
using TahaCore.DI;
using TahaCore.Reflection;
using TahaCore.Serialization.TypeParsers;

namespace TahaCore.Serialization.JsonSerialization
{
    /// <summary>
    /// Caches and provides <see cref="ITypeParser"/>s that are defined JsonTypeParserAttribute.
    /// </summary>
    public class JsonTypeParserLocator : TypeParserLocator
    {
       /// <summary>
      /// Creates a new TypeParserContext and registers all <see cref="ITypeParser"/>s that are defined in all assemblies.
      /// </summary>
      internal JsonTypeParserLocator()
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
         if (!type.IsDefined(typeof(JsonTypeParserAttribute), false)) return false;
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