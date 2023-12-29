using System;
using System.Collections.Generic;
using System.Linq;

namespace TahaCore.Reflection
{
    public static class TypeUtility
    {
        public static IEnumerable<Type> GetTypes(Predicate<Type> predicate)
        {
            var decoratedTypes = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                from type in assembly.GetTypes()
                select type).Where(type => predicate(type));

            return decoratedTypes;
        }
    }
}