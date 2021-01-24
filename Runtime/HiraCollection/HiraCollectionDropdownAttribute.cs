using System;

namespace UnityEngine
{
	public class HiraCollectionDropdownAttribute : PropertyAttribute
	{
		public HiraCollectionDropdownAttribute(Type type, bool guiEnabled = false) =>
			(Type, GUIEnabled) = (type, guiEnabled);

		public readonly Type Type;
		public readonly bool GUIEnabled;
	}
}