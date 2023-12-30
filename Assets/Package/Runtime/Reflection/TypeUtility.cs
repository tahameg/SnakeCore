// ==============================License==================================
// MIT License
// Author: Taha Mert Gökdemir
// =======================================================================
using System;
using System.Collections.Generic;
using System.Linq;

namespace TahaCore.Reflection
{
    /// <summary>
    /// Includes helper methods for System.Type class.
    /// </summary>
    public static class TypeUtility
    {
        /// <summary>
        /// Filters all types in all available assemblies with the given predicate.
        /// </summary>
        /// <param name="predicate">Filtering predicate delegate. </param>
        /// <returns>The filtered types that fits to the predicate.</returns>
        public static IEnumerable<Type> GetTypes(Predicate<Type> predicate)
        {
            var decoratedTypes = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                from type in assembly.GetTypes()
                select type).Where(type => predicate(type));

            return decoratedTypes;
        }
    }
}