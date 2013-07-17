using System;
using System.Collections.Generic;
using System.Reflection;

namespace NUnit.Framework
{
    public static class IntrospectionExtensions
    {
        public static TypeInfoEx GetTypeInfoEx(this Type type)
        {
            return new TypeInfoEx(type);
        }

        public static IEnumerable<TypeInfoEx> GetDefinedTypesEx(this Assembly assembly)
        {
#if !WinRT
            foreach (var type in assembly.GetTypes())
            {
                yield return new TypeInfoEx(type);
            }
#else
            foreach (var typeInfo in assembly.DefinedTypes)
            {
                yield return new TypeInfoEx(typeInfo);
            }
#endif
        }
    }
}