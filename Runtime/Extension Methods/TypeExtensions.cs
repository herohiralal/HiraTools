using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine
{
    public static class TypeExtensions
    {
        public static IEnumerable<Type> GetSubclasses(this Type baseClass) =>
            AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes()
                    .Where(t => baseClass.IsAssignableFrom(t) && !t.IsAbstract));

        public static IEnumerable<Type> GetSubclasses<TBase>() =>
            AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes()
                    .Where(t => (typeof(TBase)).IsAssignableFrom(t) && !t.IsAbstract));

        private static IEnumerable<string> GetNamesOfSubclasses(this IEnumerable<Type> types) =>
            types.Select(t => t.Name);

        public static IEnumerable<string> GetNamesOfSubclasses(this Type baseClass) =>
            GetSubclasses(baseClass)
                .GetNamesOfSubclasses();

        public static IEnumerable<string> GetNamesOfSubclasses<TBase>() =>
            GetSubclasses<TBase>()
                .GetNamesOfSubclasses();

        public static string GetReflectionName(this Type type) =>
            $"{type.FullName}, {type.Assembly.FullName}";
    }
}