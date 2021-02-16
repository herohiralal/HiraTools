using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace HiraEditor
{
    [CustomPropertyDrawer(typeof(HiraButtonAttribute))]
    public class HiraButtonDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!property.type.Contains(nameof(Stub)))
            {
                EditorGUI.LabelField(position, "Please only use HiraButton attribute on a Stub.");
            }
            else if (GUI.Button(position, label))
            {
                var methodInfo = property.serializedObject.targetObject.GetType()
                    .GetMethod(((HiraButtonAttribute) attribute).MethodName,
                        BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                if (methodInfo != null)
                    methodInfo.Invoke(property.serializedObject.targetObject, null);
                else
                    Debug.LogErrorFormat("There was an error finding the" +
                                         $" method {((HiraButtonAttribute) attribute).MethodName}.");
            }
        }
    }
}