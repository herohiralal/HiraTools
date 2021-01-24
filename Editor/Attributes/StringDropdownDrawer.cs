using UnityEditor;
using UnityEngine;

namespace HiraEditor.HiraAttributes
{
    [CustomPropertyDrawer(typeof(StringDropdownAttribute))]
    public class StringDropdownDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.String)
            {
                EditorGUI.HelpBox(position, $"{label.text} is not a string property.", MessageType.Error);
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

                GUI.enabled = ((StringDropdownAttribute) attribute).Editable;
                mainRect.x += 21;
                mainRect.width -= 21;
                property.stringValue = EditorGUI.TextField(mainRect, property.stringValue);
                GUI.enabled = true;
            }
            EditorGUI.indentLevel = indent;
        }

        private GenericMenu GenerateMenu(SerializedProperty property)
        {
            var menu = new GenericMenu();
            var dropdownValues = ((StringDropdownAttribute) attribute).DropdownValues;
            
            foreach (var dropdownValue in dropdownValues)
            {
                menu.AddItem(new GUIContent(dropdownValue),
                    property.stringValue == dropdownValue,
                    () =>
                    {
                        property.stringValue = dropdownValue;
                        property.serializedObject.ApplyModifiedProperties();
                    });
            }

            return menu;
        }
    }
}