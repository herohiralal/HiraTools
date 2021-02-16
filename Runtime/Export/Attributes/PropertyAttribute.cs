using System;

namespace UnityEngine
{
	[Serializable]
	public struct Stub
	{
	}
	
	[AttributeUsage(AttributeTargets.Field)]
	public class StringDropdownAttribute : PropertyAttribute
	{
		public StringDropdownAttribute(bool editable = true, params string[] dropdownValues) =>
			(Editable, DropdownValues) = (editable, dropdownValues);

		public readonly bool Editable;
		public readonly string[] DropdownValues;
	}
	
	[AttributeUsage(AttributeTargets.Field)]
	public class HiraCollectionDropdownAttribute : PropertyAttribute
	{
		public HiraCollectionDropdownAttribute(Type type, bool guiEnabled = false) =>
			(Type, GUIEnabled) = (type, guiEnabled);

		public readonly Type Type;
		public readonly bool GUIEnabled;
	}
	
	[AttributeUsage(AttributeTargets.Field)]
	public class HiraButton : PropertyAttribute
	{
		public HiraButton(string methodName) => MethodName = methodName;
		public string MethodName { get; }
	}
}