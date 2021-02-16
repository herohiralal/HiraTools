using System;
using UnityEngine;
using UnityEditor;

namespace HiraEditor
{
    [CustomPropertyDrawer(typeof(ScriptableObjectVariable), true)]
    public class ScriptableObjectVariableDrawer : PropertyDrawer
    {
        private const string variable_value_name  = "value";

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var height = EditorGUIUtility.singleLineHeight;

            if (property.objectReferenceValue == null) return height;
            
            var serializedReference = new SerializedObject(property.objectReferenceValue);
            height += EditorGUI.GetPropertyHeight(serializedReference.FindProperty(variable_value_name)) + 2;

            return height;
        }
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // rect gymnastics
            var referenceRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            var mainRect = EditorGUI.PrefixLabel(referenceRect, label);

            var bufferedHeight = EditorGUIUtility.singleLineHeight - 2;
            
            mainRect.height = bufferedHeight;

            // PrefixLabel handles indentations
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            if (property.objectReferenceValue == null)
            {
                // rect gymnastics
                var buttonRect = new Rect(mainRect.x, mainRect.y, bufferedHeight, bufferedHeight);
                mainRect.x += EditorGUIUtility.singleLineHeight;
                mainRect.width -= EditorGUIUtility.singleLineHeight;
                
                // draw button
                if (GUI.Button(buttonRect, "+")) CreateVariable(property);
            }
            else
            {
                // get the value of the object reference
                var serializedReference = new SerializedObject(property.objectReferenceValue);
                var serializedValue = serializedReference.FindProperty(variable_value_name);
                
                // rect gymnastics
                var valueRect = new Rect(
                    mainRect.x - EditorGUIUtility.singleLineHeight,
                    position.y + EditorGUIUtility.singleLineHeight,
                    mainRect.width + EditorGUIUtility.singleLineHeight,
                    EditorGUI.GetPropertyHeight(serializedValue));

                // draw the value property
                EditorGUI.BeginChangeCheck();
                EditorGUI.PropertyField(valueRect, serializedValue, GUIContent.none);
                if (EditorGUI.EndChangeCheck()) serializedReference.ApplyModifiedProperties();
            }

            // draw the main property
            EditorGUI.PropertyField(mainRect, property, GUIContent.none);

            EditorGUI.indentLevel = indent;
        }
        
        #region "+" Button
        
        private static readonly string namespace_for_scriptable_object_variables =
            typeof(ScriptableObjectVariable).Namespace?.Replace(".Core", "");
        private static readonly string assembly_for_scriptable_object_variables =
            typeof(ScriptableObjectVariable).Assembly.FullName;

        private static void CreateVariable(SerializedProperty property)
        {
            var path = EditorUtility.SaveFilePanelInProject("Save File", "New Object",
                "asset", "Save file to...");

            if (string.IsNullOrEmpty(path)) return;
            
            var variablePath = namespace_for_scriptable_object_variables + "." +
                               property.type.Replace("PPtr<$", "").Replace(">", "")
                               + ", " + assembly_for_scriptable_object_variables;
                
            property.objectReferenceValue = ScriptableObject.CreateInstance(Type.GetType(variablePath));
            AssetDatabase.CreateAsset(property.objectReferenceValue, path);
        }
        #endregion
    }
}