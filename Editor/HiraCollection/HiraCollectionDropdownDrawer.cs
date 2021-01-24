using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace HiraEditor.HiraAttributes
{
	[CustomPropertyDrawer(typeof(HiraCollectionDropdownAttribute))]
	public class HiraCollectionDropdownDrawer : PropertyDrawer
	{
		private HiraCollectionDropdownAttribute Attribute => (HiraCollectionDropdownAttribute) attribute;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (property.propertyType != SerializedPropertyType.ObjectReference)
			{
				EditorGUI.HelpBox(position, $"{label.text} is not an object property.", MessageType.Error);
				return;
			}

			var mainRect = EditorGUI.PrefixLabel(position, label);

			var indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;
			{
				var buttonRect = mainRect;
				buttonRect.width = 20;

				if (EditorGUI.DropdownButton(buttonRect, GUIContent.none, FocusType.Passive))
					GenerateMenu(property).DropDown(buttonRect);

				mainRect.x += 21;
				mainRect.width -= 21;

				GUI.enabled = Attribute.GUIEnabled;
				{
					EditorGUI.ObjectField(mainRect, GUIContent.none, property.objectReferenceValue, Attribute.Type,
						false);
				}
				GUI.enabled = true;
			}
			EditorGUI.indentLevel = indent;
		}

		private GenericMenu GenerateMenu(SerializedProperty property)
		{
			var currentValue = property.objectReferenceValue;
			var menu = new GenericMenu();

#if UNITY_EDITOR && !STRIP_EDITOR_CODE
			var hiraCollectionTypes = Attribute.Type.GetHierarchy(true, true)
				.Select(t => typeof(HiraCollection<>).MakeGenericType(t))
				.SelectMany(t=>t.GetSubclasses());

			if (currentValue == null)
				menu.AddDisabledItem("Clear".GetGUIContent(), true);
			else menu.AddItem("Clear".GetGUIContent(), false, () =>
			{
				property.objectReferenceValue = null;
				property.serializedObject.ApplyModifiedProperties();
			});
			
			foreach (var hiraCollectionType in hiraCollectionTypes)
			{
				menu.AddSeparator("");
				
				foreach (var hiraCollection in Resources.FindObjectsOfTypeAll(hiraCollectionType))
				foreach (var o in ((IHiraCollectionEditorInterface) hiraCollection).Collection1)
					if (currentValue == o)
						menu.AddDisabledItem(new GUIContent($"{hiraCollection.name}/{o.name}"), true);
					else if (Attribute.Type.IsInstanceOfType(o))
						menu.AddItem(new GUIContent($"{hiraCollection.name}/{o.name}"), false, () =>
						{
							property.objectReferenceValue = o;
							property.serializedObject.ApplyModifiedProperties();
						});
					else
						menu.AddDisabledItem(new GUIContent($"{hiraCollection.name}/{o.name}"), false);
			}
#endif

			return menu;
		}
	}
}