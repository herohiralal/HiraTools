using HiraEditor;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

namespace LGOAPDemo
{
    [CustomEditor(typeof(HiraBlackboardComponent))]
    public class BlackboardComponentEditor : Editor
    {
        private const string core_property_name = "core";
        private const string template_property_name = "template";
        private const string broadcast_key_update_events_property_name = "broadcastKeyUpdateEvents";

        public IBlackboardComponent Target => (IBlackboardComponent) target;

        public override void OnInspectorGUI()
        {
            var templateProperty = serializedObject.FindProperty(core_property_name).FindPropertyRelative(template_property_name);
            var broadcastKeyUpdateEventsProperty = serializedObject.FindProperty(core_property_name).FindPropertyRelative(broadcast_key_update_events_property_name);

            if (!Target.Data.IsCreated)
            {
                if (templateProperty != null) EditorGUILayout.PropertyField(templateProperty);
                else
                {
                    EditorGUILayout.HelpBox($"{template_property_name} property not found.", MessageType.Error);
                    return;
                }

                if (broadcastKeyUpdateEventsProperty != null) EditorGUILayout.PropertyField(broadcastKeyUpdateEventsProperty);
                else
                {
                    EditorGUILayout.HelpBox($"{broadcast_key_update_events_property_name} property not found.", MessageType.Error);
                    return;
                }

                if (serializedObject.hasModifiedProperties)
                    serializedObject.ApplyModifiedProperties();
            }
            else
            {
                if (templateProperty.propertyType != SerializedPropertyType.ObjectReference || templateProperty.objectReferenceValue == null)
                {
                    EditorGUILayout.HelpBox("Template property not recognized.", MessageType.Error);
                    return;
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    var controlRect = EditorGUILayout.GetControlRect();
                    controlRect = EditorGUI.PrefixLabel(controlRect, "Key".GetGUIContent(), EditorStyles.largeLabel);
                    EditorGUI.LabelField(controlRect, "Value".GetGUIContent(), EditorStyles.largeLabel);
                }

                unsafe
                {
                    var template = (HiraBlackboardTemplate) templateProperty.objectReferenceValue;
                    var data = (byte*) Target.Data.GetUnsafeReadOnlyPtr();

                    foreach (var key in template.Collection1)
                        key.DrawEditor(data + key.Index, Target);
                }
            }
        }
    }
}