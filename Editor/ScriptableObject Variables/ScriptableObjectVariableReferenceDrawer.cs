using UnityEngine;
using UnityEditor;

namespace Hiralal.SOVariables.Core.Editor
{
    [CustomPropertyDrawer(typeof(ScriptableObjectVariableReference), true)]
    public class ScriptableObjectVariableReferenceDrawer : PropertyDrawer
    {
        private const string use_constant_property_name = "useConstant";
        private const string constant_value_property_name = "constant";
        private const string variable_value_property_name = "variable";
        
        private static readonly string[] popup_options = { "Use Constant", "Use Variable" };
        private GUIStyle _popupStyle;
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var constantProperty = property.FindPropertyRelative(use_constant_property_name);
            var variableProperty = property.FindPropertyRelative(variable_value_property_name);

            return constantProperty.boolValue || variableProperty.objectReferenceValue == null
                ? EditorGUI.GetPropertyHeight(constantProperty)
                : EditorGUI.GetPropertyHeight(variableProperty);
        }
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (_popupStyle == null)
                _popupStyle = new GUIStyle(GUI.skin.GetStyle("PaneOptions")) {imagePosition = ImagePosition.ImageOnly};

            var useConstantProperty = property.FindPropertyRelative(use_constant_property_name);
            var constantProperty = property.FindPropertyRelative(constant_value_property_name);
            var variableProperty = property.FindPropertyRelative(variable_value_property_name);
            
            // rect gymnastics
            var mainRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

            // prefix label
            mainRect = EditorGUI.PrefixLabel(mainRect, label);
            
            // more rect gymnastics
            var buttonRect = new Rect(mainRect.x, mainRect.y, EditorGUIUtility.singleLineHeight, mainRect.height);
            mainRect.x += EditorGUIUtility.singleLineHeight + 2;
            mainRect.width -= EditorGUIUtility.singleLineHeight + 2;

            // PrefixLabel handles indentation
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // get UseConstant value
            useConstantProperty.boolValue = 
                EditorGUI.Popup(buttonRect, useConstantProperty.boolValue ? 0 : 1, popup_options, _popupStyle) == 0;

            // rect gymnastics idk
            if (!useConstantProperty.boolValue) mainRect.height = EditorGUIUtility.singleLineHeight * 2;

            EditorGUI.PropertyField(mainRect, useConstantProperty.boolValue ? constantProperty : variableProperty, GUIContent.none);
            
            EditorGUI.indentLevel = indent;
        }
    }
}