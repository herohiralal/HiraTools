using System.Collections.Generic;
using System.Linq;

namespace System
{
	public static class TypeExtensions
	{
		public static IEnumerable<Type> AllTypes =>
			AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(assembly => assembly.GetTypes());

		public static IEnumerable<string> GetNamesOfClasses(this IEnumerable<Type> types) =>
			types.Select(t => t.Name);

		public static IEnumerable<Type> GetSubClasses(this Type baseClass, bool allowAbstract = false,
			bool allowSelf = true) =>
			AllTypes.Where(t =>
				baseClass.IsAssignableFrom(t) && (allowAbstract || !t.IsAbstract) && (allowSelf || t != baseClass));

		public static IEnumerable<Type> GetBaseTypes(this Type childClass, bool allowAbstract = false,
			bool allowSelf = true) =>
			AllTypes.Where(t =>
				t.IsAssignableFrom(childClass) && t != typeof(object) && (allowAbstract || !t.IsAbstract) &&
				(allowSelf || t != childClass));

		public static IEnumerable<Type> GetHierarchy(this Type target, bool allowAbstract = true,
			bool includeSelf = true) =>
			target.GetBaseTypes(allowAbstract, false)
				.Concat(target.GetSubClasses(allowAbstract, includeSelf));

		public static string GetReflectionName(this Type type) =>
			$"{type.FullName}, {type.Assembly.FullName}";
	}
}