using System;
using System.Reflection;
using static UnityEngine.DynamicEnumValue;
using UnityEditor;
using UnityEngine;

namespace HiraEditor
{
	[CustomPropertyDrawer(typeof(DynamicEnumValue))]
	public class DynamicEnumValueDrawer : PropertyDrawer
	{
		private const string type_property_name = "type";
		private const string byte_value_property_name = "byteValue";
		private const string s_byte_value_property_name = "sByteValue";
		private const string u_short_value_property_name = "uShortValue";
		private const string short_value_property_name = "shortValue";
		private const string u_int_value_property_name = "uIntValue";
		private const string int_value_property_name = "intValue";
		private const string u_long_value_property_name = "uLongValue";
		private const string long_value_property_name = "longValue";

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) =>
			2 * base.GetPropertyHeight(property, label);

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var topRect = position;
			var bottomRect = position;

			topRect.height *= 0.5f;
			topRect.height -= 1;

			bottomRect.height *= 0.5f;
			bottomRect.height -= 1;
			bottomRect.y += bottomRect.height + 1;

			topRect = EditorGUI.PrefixLabel(topRect, label);
			bottomRect.x = topRect.x;
			bottomRect.width = topRect.width;

			var indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;
			var typeProperty = property.FindPropertyRelative(type_property_name);
			if (typeProperty == null)
			{
				EditorGUI.LabelField(topRect, $"Could not find property {type_property_name}.");
				return;
			}

			{
				var buttonRect = topRect;
				buttonRect.width = 20;
				if (EditorGUI.DropdownButton(buttonRect, GUIContent.none, FocusType.Passive))
					GenerateMenu(typeProperty).DropDown(buttonRect);
				topRect.x += 21;
				topRect.width -= 21;
			}

			var typePropertyGuid = typeProperty.stringValue;
			if (!DynamicallyAccessibleEnumAttribute.DATABASE.ContainsKey(typePropertyGuid))
			{
				EditorGUI.LabelField(topRect, $"Could not find enum {typeProperty.stringValue}.");
				return;
			}

			var enumType = DynamicallyAccessibleEnumAttribute.DATABASE[typePropertyGuid];

			var guiEnabled = GUI.enabled;
			GUI.enabled = false;
			EditorGUI.TextField(topRect, enumType.FullName);
			GUI.enabled = guiEnabled;

			var enumUnderlyingType = Enum.GetUnderlyingType(enumType);

			if (enumUnderlyingType == typeof(byte))
			{
				var valueProperty = property.FindPropertyRelative(byte_value_property_name);
				valueProperty.intValue = DynamicEnumPopup(bottomRect, (byte) valueProperty.intValue, enumType);
			}
			else if (enumUnderlyingType == typeof(sbyte))
			{
				var valueProperty = property.FindPropertyRelative(s_byte_value_property_name);
				valueProperty.intValue = DynamicEnumPopup(bottomRect, (sbyte) valueProperty.intValue, enumType);
			}
			else if (enumUnderlyingType == typeof(ushort))
			{
				var valueProperty = property.FindPropertyRelative(u_short_value_property_name);
				valueProperty.intValue = DynamicEnumPopup(bottomRect, (ushort) valueProperty.intValue, enumType);
			}
			else if (enumUnderlyingType == typeof(short))
			{
				var valueProperty = property.FindPropertyRelative(short_value_property_name);
				valueProperty.intValue = DynamicEnumPopup(bottomRect, (short) valueProperty.intValue, enumType);
			}
			else if (enumUnderlyingType == typeof(uint))
			{
				var valueProperty = property.FindPropertyRelative(u_int_value_property_name);
				valueProperty.longValue = DynamicEnumPopup(bottomRect, (uint) valueProperty.longValue, enumType);
			}
			else if (enumUnderlyingType == typeof(int))
			{
				var valueProperty = property.FindPropertyRelative(int_value_property_name);
				valueProperty.intValue = DynamicEnumPopup(bottomRect, valueProperty.intValue, enumType);
			}
			else if (enumUnderlyingType == typeof(ulong))
			{
				var valueProperty = property.FindPropertyRelative(u_long_value_property_name);
				valueProperty.longValue = (long) DynamicEnumPopup(bottomRect, (ulong) valueProperty.longValue, enumType);
			}
			else if (enumUnderlyingType == typeof(long))
			{
				var valueProperty = property.FindPropertyRelative(long_value_property_name);
				valueProperty.longValue = DynamicEnumPopup(bottomRect, valueProperty.longValue, enumType);
			}

			EditorGUI.indentLevel = indent;
		}

		private static T DynamicEnumPopup<T>(Rect rect, T inValue, System.Type enumType)
		{
			var enumCast = RuntimeCastToEnum(inValue, enumType);
			var output =
				enumType.GetCustomAttribute<FlagsAttribute>() == null
					? EditorGUI.EnumPopup(rect, GUIContent.none, enumCast)
					: EditorGUI.EnumFlagsField(rect, GUIContent.none, enumCast);
			return RuntimeCastToUnderlyingType<T>(output);
		}

		private static GenericMenu GenerateMenu(SerializedProperty property)
		{
			var menu = new GenericMenu();

			foreach (var kvp in DynamicallyAccessibleEnumAttribute.DATABASE)
			{
				var dropdownValueToShow = kvp.Value.AssemblyQualifiedName;
				menu.AddItem(new GUIContent(kvp.Value.FullName?.Replace('.', '/')),
					property.stringValue == dropdownValueToShow,
					() =>
					{
						property.stringValue = kvp.Key;
						property.serializedObject.ApplyModifiedProperties();
					});
			}

			return menu;
		}
	}
}