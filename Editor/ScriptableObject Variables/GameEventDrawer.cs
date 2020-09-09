﻿using UnityEngine;

namespace UnityEditor
{
    [CustomPropertyDrawer(typeof(GameEvent))]
    public class GameEventDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => 
            base.GetPropertyHeight(property, label) + EditorGUIUtility.singleLineHeight + 2;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var propertyRect = new Rect(position.x, position.y, position.width,
                position.height - EditorGUIUtility.singleLineHeight);

            EditorGUI.PropertyField(propertyRect, property, label, true);

            GUI.enabled = Application.isPlaying && property.objectReferenceValue != null;
            DrawTestButton(position, property);
            GUI.enabled = true;
        }

        private static void DrawTestButton(Rect position, SerializedProperty property)
        {
            var testButtonPosition = new Rect(position.x,
                position.y + position.height - EditorGUIUtility.singleLineHeight - 1,
                position.width,
                EditorGUIUtility.singleLineHeight);
            if (GUI.Button(EditorGUI.IndentedRect(testButtonPosition), "Raise")) 
                ((GameEvent) (property.objectReferenceValue)).Raise();
        }
    }
}