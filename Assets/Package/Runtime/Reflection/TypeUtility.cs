// ==============================License==================================
// MIT License
// Author: Taha Mert Gökdemir
// =======================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SnakeCore.Reflection
{
    /// <summary>
    /// Includes helper methods for System.Type class.
    /// </summary>
    public static class TypeUtility
    {
        private static IEnumerable<Assembly> assemblies;
        private static IEnumerable<Type> types;
        
        /// <summary>
        /// Filters all types in all available assemblies with the given predicate.
        /// </summary>
        /// <param name="predicate">Filtering predicate delegate. </param>
        /// <returns>The filtered types that fits to the predicate.</returns>
        public static IEnumerable<Type> GetTypes(Predicate<Type> predicate)
        {
            GetTypesIfEmpty();
            var decoratedTypes = types.Where(type => predicate(type));

            return decoratedTypes;
        }
        
        
        public static Type GetType(string typeName)
        {
            GetAssembliesIfEmpty();
            foreach (Assembly assembly in assemblies)
            {
                Type type = assembly.GetType(typeName);
                if (type != null)
                    return type;
            }
            return null;
        }
        
        private static void GetAssembliesIfEmpty()
        {
            if(assemblies != null) return;
            assemblies = AppDomain.CurrentDomain.GetAssemblies();
        }
        
        private static void GetTypesIfEmpty()
        {
            if(types != null) return;
            GetAssembliesIfEmpty();
            types = assemblies.SelectMany(a => a.GetTypes()).ToArray();
        }
    }
}