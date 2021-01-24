using System.Collections.Generic;
using System.Linq;

namespace System
{
    public static class TypeExtensions
    {
        public static IEnumerable<string> GetNamesOfClasses(this IEnumerable<Type> types) =>
            types.Select(t => t.Name);
        
        public static IEnumerable<Type> GetSubclasses(this Type baseClass) =>
            AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes()
                    .Where(t => baseClass.IsAssignableFrom(t) && !t.IsAbstract));

        public static IEnumerable<Type> GetSubclasses<TBase>() =>
            typeof(TBase).GetSubclasses();

        public static IEnumerable<string> GetNamesOfSubclasses(this Type baseClass) =>
            GetSubclasses(baseClass)
                .GetNamesOfClasses();

        public static IEnumerable<string> GetNamesOfSubclasses<TBase>() =>
            GetSubclasses<TBase>()
                .GetNamesOfClasses();

        public static IEnumerable<Type> GetAncestorTypes(this Type baseClass) =>
            AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(t => t.IsAssignableFrom(baseClass) && t != typeof(object));

        public static IEnumerable<Type> GetAncestorTypes<TBase>() =>
            typeof(TBase).GetAncestorTypes();

        public static IEnumerable<string> GetNamesOfAncestorTypes(this Type baseClass) =>
            GetAncestorTypes(baseClass)
                .GetNamesOfClasses();

        public static IEnumerable<string> GetNamesOfAncestorTypes<TBase>() =>
            GetAncestorTypes<TBase>()
                .GetNamesOfClasses();

        public static string GetReflectionName(this Type type) =>
            $"{type.FullName}, {type.Assembly.FullName}";
    }
}