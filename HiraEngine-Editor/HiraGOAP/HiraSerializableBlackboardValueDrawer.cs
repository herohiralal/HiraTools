using System;
using Hiralal.Blackboard;
using Hiralal.GOAP;
using UnityEngine;

namespace UnityEditor
{
    [CustomPropertyDrawer(typeof(HiraSerializableBlackboardValue))]
    public class HiraSerializableBlackboardValueDrawer : PropertyDrawer
    {
        private const string key_name_variable_name = "name";
        private const string key_type_variable_name = "keyType";
        private const string bool_value_variable_name = "boolValue";
        private const string float_value_variable_name = "floatValue";
        private const string int_value_variable_name = "intValue";
        private const string string_value_variable_name = "stringValue";
        private const string vector_value_variable_name = "vectorValue";

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) =>
            (EditorGUIUtility.singleLineHeight * 2) + 4
                                                    + (property.FindPropertyRelative(key_name_variable_name)
                                                        .FindPropertyRelative("useConstant").boolValue
                                                        ? 0
                                                        : (EditorGUIUtility.singleLineHeight + 2));

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var firstLineRect = position.KeepToTopFor((int) (EditorGUIUtility.singleLineHeight + 2));

            firstLineRect = EditorGUI.PrefixLabel(firstLineRect, label);
            var nameProperty = property.FindPropertyRelative(key_name_variable_name);
            EditorGUI.PropertyField(firstLineRect, nameProperty, GUIContent.none);

            var prefixWidth = (int) (position.width - firstLineRect.width);

            var secondLineRect = position.ShiftToBottomBy((int) (EditorGUIUtility.singleLineHeight + 2 + (nameProperty
                .FindPropertyRelative("useConstant").boolValue
                ? 0
                : (EditorGUIUtility.singleLineHeight + 2))));

            var keyTypeProperty = property.FindPropertyRelative(key_type_variable_name);
            EditorGUI.PropertyField(secondLineRect.KeepToLeftFor(prefixWidth).ShiftToRightBy(10),
                keyTypeProperty, GUIContent.none);

            var valuePropertyPath = ChoosePropertyPath((HiraBlackboardKeyType) keyTypeProperty.enumValueIndex);

            if (keyTypeProperty.enumValueIndex != 0)
                EditorGUI.PropertyField(secondLineRect.ShiftToRightBy(prefixWidth),
                    property.FindPropertyRelative(valuePropertyPath), GUIContent.none);
        }

        private static string ChoosePropertyPath(HiraBlackboardKeyType keyType)
        {
            // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
            switch (keyType)
            {
                case HiraBlackboardKeyType.Bool:
                    return bool_value_variable_name;
                case HiraBlackboardKeyType.Float:
                    return float_value_variable_name;
                case HiraBlackboardKeyType.Int:
                    return int_value_variable_name;
                case HiraBlackboardKeyType.String:
                    return string_value_variable_name;
                case HiraBlackboardKeyType.Vector:
                    return vector_value_variable_name;
            }

            return null;
        }
    }
}