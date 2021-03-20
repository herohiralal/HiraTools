using UnityEditor;
using UnityEngine;

namespace LGOAPDemo
{
    [CustomEditor(typeof(HiraSharedBlackboardComponent))]
    public class SharedBlackboardComponentEditor : BlackboardComponentEditor
    {
        private const string shared_blackboard_property_name = "sharedBlackboard";

        public override void OnInspectorGUI()
        {
            if (Application.isPlaying)
                base.OnInspectorGUI();
            else
            {
                var sharedBlackboardProperty = serializedObject.FindProperty(shared_blackboard_property_name);
                if (sharedBlackboardProperty != null)
                {
                    EditorGUILayout.PropertyField(sharedBlackboardProperty);

                    if (serializedObject.hasModifiedProperties)
                        serializedObject.ApplyModifiedProperties();
                }
                else
                {
                    EditorGUILayout.HelpBox($"{shared_blackboard_property_name} not found.", MessageType.Error);
                }
            }
        }
    }
}