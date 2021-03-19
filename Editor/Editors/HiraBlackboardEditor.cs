using HiraEditor;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

namespace LGOAPDemo
{
    [CustomEditor(typeof(HiraBlackboard))]
    public class BlackboardEditor : Editor
    {
        private const string template_property_name = "template";
        private const string broadcast_key_update_events_property_name = "broadcastKeyUpdateEvents";

        public HiraBlackboard Target => (HiraBlackboard) target;

        public override void OnInspectorGUI()
        {
            var templateProperty = serializedObject.FindProperty(template_property_name);
            var broadcastKeyUpdateEventsProperty = serializedObject.FindProperty(broadcast_key_update_events_property_name);

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

            if (!Target.Data.IsCreated) return;

            if (templateProperty.propertyType != SerializedPropertyType.ObjectReference || templateProperty.objectReferenceValue == null)
            {
                EditorGUILayout.HelpBox("Template property not recognized.", MessageType.Error);
                return;
            }

            GUI.enabled = false;
            unsafe
            {
                var template = Target.template;
                var data = (byte*) Target.Data.GetUnsafeReadOnlyPtr();

                foreach (var key in template.Collection1)
                {
                    var value = key.GetValue(data + key.Index);
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        var controlRect = EditorGUILayout.GetControlRect();
                        controlRect = EditorGUI.PrefixLabel(controlRect, $"{key.name}".GetGUIContent());
                        EditorGUI.SelectableLabel(controlRect, value);
                    }
                }
            }

            GUI.enabled = true;
        }
    }
}