using System;

namespace UnityEngine
{
	[AttributeUsage(AttributeTargets.Class)]
	public class EditorOnlyGameObjectAttribute : Attribute
	{
	}

	[AttributeUsage(AttributeTargets.Class)]
	public class EditorOnlyComponentAttribute : Attribute
	{
	}

	public static class ConditionalComponentHelpers
	{
		public static bool IsComponentEditorOnly(Type t) =>
			t.GetCustomAttributes(typeof(EditorOnlyComponentAttribute), false).Length > 0;

		public static bool IsGameObjectEditorOnly(Type t) =>
			t.GetCustomAttributes(typeof(EditorOnlyGameObjectAttribute), false).Length > 0;
	}
}