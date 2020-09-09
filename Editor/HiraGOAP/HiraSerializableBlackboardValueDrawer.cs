﻿using Hiralal.Blackboard;
using Hiralal.GOAP.Transitions;
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

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // name height
            var height = EditorGUI.GetPropertyHeight(property.FindPropertyRelative(key_name_variable_name));

            // value height
            var typeProperty = property.FindPropertyRelative(key_type_variable_name);
            var valueProperty = ChooseValueProperty(property, typeProperty.enumValueIndex);
            height += valueProperty == null
                ? EditorGUI.GetPropertyHeight(typeProperty)
                : EditorGUI.GetPropertyHeight(valueProperty);
            
            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // name property
            var nameProperty = property.FindPropertyRelative(key_name_variable_name);

            // main rect calculations
            var firstLineRect = position.KeepToTopFor((int) EditorGUI.GetPropertyHeight(nameProperty));
            var secondLineRect = position.ShiftToBottomBy((int) firstLineRect.height);

            // label
            firstLineRect = EditorGUI.PrefixLabel(firstLineRect, label);
            
            // name
            EditorGUI.PropertyField(firstLineRect, nameProperty, GUIContent.none);

            // label rect calculations
            var prefixWidth = (int) (position.width - firstLineRect.width);
            
            // type
            var keyTypeProperty = property.FindPropertyRelative(key_type_variable_name);
            EditorGUI.PropertyField(secondLineRect.KeepToLeftFor(prefixWidth).ShiftToRightBy(10),
                keyTypeProperty, GUIContent.none);

            // value
            var valueProperty = ChooseValueProperty(property, keyTypeProperty.enumValueIndex);
            if (keyTypeProperty.enumValueIndex != 0)
                EditorGUI.PropertyField(secondLineRect.ShiftToRightBy(prefixWidth), valueProperty, GUIContent.none);
        }

        private static SerializedProperty ChooseValueProperty(SerializedProperty property,
            int keyType) =>
            (HiraBlackboardKeyType) keyType switch
            {
                HiraBlackboardKeyType.Bool => property.FindPropertyRelative(bool_value_variable_name),
                HiraBlackboardKeyType.Float => property.FindPropertyRelative(float_value_variable_name),
                HiraBlackboardKeyType.Int => property.FindPropertyRelative(int_value_variable_name),
                HiraBlackboardKeyType.String => property.FindPropertyRelative(string_value_variable_name),
                HiraBlackboardKeyType.Vector => property.FindPropertyRelative(vector_value_variable_name),
                _ => null
            };
    }
}