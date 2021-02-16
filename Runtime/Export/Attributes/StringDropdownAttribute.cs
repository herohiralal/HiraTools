using System;

namespace UnityEngine
{
	[AttributeUsage(AttributeTargets.Field)]
	public class StringDropdownAttribute : PropertyAttribute
	{
		public StringDropdownAttribute(bool editable = true, params string[] dropdownValues) =>
			(Editable, DropdownValues) = (editable, dropdownValues);

		public readonly bool Editable;
		public readonly string[] DropdownValues;
	}
}