using System;

namespace HiraEditor.Internal
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class HiraCollectionTargetEditorAttribute : Attribute
	{
		public HiraCollectionTargetEditorAttribute(Type targetType)
		{
			TargetType = targetType;
		}
		
		public readonly Type TargetType;
	}
}