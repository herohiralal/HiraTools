using System;
using System.Linq;
using HiraEditor.HiraEngine.Components.Blackboard.Helpers;
using UnityEditor;
using UnityEngine;

namespace HiraEditor.HiraEngine.Components.Blackboard
{
    [CustomPropertyDrawer(typeof(SerializableBlackboardValue))]
    public class SerializableBlackboardValueDrawer : PropertyDrawer
    {
        private const string key_set_property_name = "keySet";
        private const string key_property_name = "key";
        private const string type_string_property_name = "typeString";
        private const string bool_value_property_name = "boolValue";
        private const string float_value_property_name = "floatValue";
        private const string int_value_property_name = "intValue";
        private const string string_value_property_name = "stringValue";
        private const string vector_value_property_name = "vectorValue";

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var height = 0f;

            var keySet =
                (HiraBlackboardKeySet) property.FindPropertyRelative(key_set_property_name).objectReferenceValue;
            if (keySet != null && keySet.Keys.Length != 0)
                height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative(key_property_name)) + 2;

            var chosenProperty = ChooseValueProperty(property);

            if (chosenProperty != null)
            {
                height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative(type_string_property_name)) + 2;
                height += EditorGUI.GetPropertyHeight(chosenProperty) + 2;
            }

            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // initializing properties
            var keySetProperty = property.FindPropertyRelative(key_set_property_name);
            var parentKeySet = property.serializedObject.FindProperty(key_set_property_name)?.objectReferenceValue;
            keySetProperty.objectReferenceValue = parentKeySet;

            var keyProperty = property.FindPropertyRelative(key_property_name);
            if (parentKeySet == null || ((HiraBlackboardKeySet) parentKeySet).Keys.Length == 0)
            {
                keyProperty.objectReferenceValue = null;
                return;
            }

            // adjust padding
            position.y += 1;
            position.height -= 2;

            // backup label because need a fucking steparound thanks unity for having issues in your code.
            var backupLabel = new GUIContent(label.text, label.image, label.tooltip);

            // setup rects
            var firstLineRect =
                EditorGUI.PrefixLabel(position.KeepToTopFor((int) EditorGUI.GetPropertyHeight(keyProperty)),
                    backupLabel);

            // reset indent
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // key dropdown
            {
                var (names, keys) = keySetProperty.objectReferenceValue.BuildKeyData();

                if (names == null || names.Length == 0 || keys == null || keys.Length == 0)
                {
                    keyProperty.objectReferenceValue = null;
                    return;
                }

                var index = keyProperty.objectReferenceValue != null && keys.Contains(keyProperty.objectReferenceValue)
                    ? Array.IndexOf(keys, keyProperty.objectReferenceValue)
                    : 0;

                keyProperty.objectReferenceValue = keys[EditorGUI.Popup(firstLineRect, index, names)];
            }

            // value field
            {
                var valueProperty = ChooseValueProperty(property);
                if (valueProperty != null)
                {
                    // rect setup
                    var valueRect = position.KeepToBottomFor((int) EditorGUI.GetPropertyHeight(valueProperty));
                    valueRect.x = firstLineRect.x;
                    valueRect.width = firstLineRect.width;

                    EditorGUI.PropertyField(valueRect, valueProperty, GUIContent.none);
                }
                else return;
            }

            // calculation type
            {
                var typeStringProperty = property.FindPropertyRelative(type_string_property_name);
                var (displayNames, reflectionNames) = keyProperty.objectReferenceValue.BuildCalculationData();

                if (displayNames == null || displayNames.Length == 0 || reflectionNames == null ||
                    reflectionNames.Length == 0)
                {
                    typeStringProperty.stringValue = "";
                    return;
                }

                var index = reflectionNames.Contains(typeStringProperty.stringValue)
                    ? Array.IndexOf(reflectionNames, typeStringProperty.stringValue)
                    : 0;

                // rect setup
                var typeRect = position.ShiftToBottomBy((int) firstLineRect.height + 2)
                    .KeepToTopFor((int) EditorGUI.GetPropertyHeight(typeStringProperty));
                typeRect.x = firstLineRect.x;
                typeRect.width = firstLineRect.width;

                typeStringProperty.stringValue = reflectionNames[EditorGUI.Popup(typeRect, index, displayNames)];
            }

            // undo indent reset
            EditorGUI.indentLevel = indent;
        }

        private static SerializedProperty ChooseValueProperty(SerializedProperty property)
        {
            var key = property.FindPropertyRelative(key_property_name).objectReferenceValue;
            if (key == null) return null;
            if (!(key is SerializableBlackboardKey serializableKey)) return null;

            string propertyName;
            switch (serializableKey.KeyType)
            {
                case BlackboardKeyType.Bool:
                    propertyName = bool_value_property_name;
                    break;
                case BlackboardKeyType.Float:
                    propertyName = float_value_property_name;
                    break;
                case BlackboardKeyType.Int:
                    propertyName = int_value_property_name;
                    break;
                case BlackboardKeyType.String:
                    propertyName = string_value_property_name;
                    break;
                case BlackboardKeyType.Vector:
                    propertyName = vector_value_property_name;
                    break;
                default: return null;
            }

            return property.FindPropertyRelative(propertyName);
        }
    }
}