using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace HiraEditor
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

			var hierarchy = Attribute.Type.GetHierarchy().ToArray();
			var data = HiraCollectionEditor.HIRA_COLLECTION_TARGET_TYPES;
			var targetHiraCollectionTypes = new[] {new List<Type>(), new List<Type>(), new List<Type>()};
			foreach (var keyValuePair in data)
			{
				var collection = keyValuePair.Key;
				var targets = keyValuePair.Value;
				var targetsLength = targets.Length;
				for (var i = 0; i < 3; i++)
				{
					if (i >= targetsLength) continue;

					if (hierarchy.Any(type => targets[i].IsAssignableFrom(type)))
					{
						targetHiraCollectionTypes[i].Add(collection);
					}
				}
			}
			
			if (currentValue == null)
				menu.AddDisabledItem("Clear".GetGUIContent(), true);
			else menu.AddItem("Clear".GetGUIContent(), false, () =>
			{
				property.objectReferenceValue = null;
				property.serializedObject.ApplyModifiedProperties();
			});
			
			for (var i = 0; i < targetHiraCollectionTypes.Length; i++)
			{
				var hiraCollectionTypes = targetHiraCollectionTypes[i];
				foreach (var hiraCollectionType in hiraCollectionTypes)
				{
					foreach (var hiraCollection in hiraCollectionType.TrueFindObjectsOfTypeAll())
					foreach (var o in ((IHiraCollectionEditorInterface) hiraCollection).CollectionInternal[i])
						if (currentValue == o)
							menu.AddDisabledItem(new GUIContent($"{hiraCollectionType.Name}/{hiraCollection.name}/{o.name}"), true);
						else if (Attribute.Type.IsInstanceOfType(o))
							menu.AddItem(new GUIContent($"{hiraCollectionType.Name}/{hiraCollection.name}/{o.name}"), false, () =>
							{
								property.objectReferenceValue = o;
								property.serializedObject.ApplyModifiedProperties();
						});
						else
							menu.AddDisabledItem(new GUIContent($"{hiraCollectionType.Name}/{hiraCollection.name}/{o.name}"), false);
			    }
			}
#endif

			return menu;
		}
	}
}