﻿using System.Reflection;
using UnityEngine;

namespace UnityEditor
{
    [CustomPropertyDrawer(typeof(HiraButton))]
    public class HiraButtonDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!property.type.Contains(nameof(HiraButtonToken)))
            {
                EditorGUI.LabelField(position, "Please only use HiraButton attribute on a HiraToken.");
            }
            else if (GUI.Button(position, label))
            {
                var methodInfo = property.serializedObject.targetObject.GetType()
                    .GetMethod(((HiraButton) attribute).MethodName,
                        BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                if (methodInfo != null)
                    methodInfo.Invoke(property.serializedObject.targetObject, null);
                else
                    Debug.LogErrorFormat("There was an error finding the" +
                                         $" method {((HiraButton) attribute).MethodName}.");
            }
        }
    }
}