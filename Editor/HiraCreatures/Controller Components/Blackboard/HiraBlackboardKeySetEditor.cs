using UnityEditor;
using UnityEngine;

namespace HiraEditor.HiraEngine.Components.Blackboard
{
    [CustomEditor(typeof(HiraBlackboardKeySet))]
    public class HiraBlackboardKeySetEditor : HiraCollectionEditor
    {
        private const string key_type_property_name = "keyType";
        private const string instance_synchronized_property_name = "instanceSynchronized";

        protected override void DrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            rect.height -= 2;
            var currentSerializedObject = new SerializedObject(ObjectAtIndex(index));

            var namePropertyValue = currentSerializedObject.targetObject.name;
            var keyTypeProperty = currentSerializedObject.FindProperty(key_type_property_name);
            var keyTypePropertyValue = keyTypeProperty.enumValueIndex;
            var instanceSyncedProperty = currentSerializedObject.FindProperty(instance_synchronized_property_name);
            var instanceSyncedPropertyValue = instanceSyncedProperty.boolValue;

            var instanceSyncedPropertyNewValue =
                GUI.Toggle(rect.KeepToLeftFor(30).ShiftToRightBy(5), instanceSyncedPropertyValue, GUIContent.none);
            var namePropertyNewValue =
                EditorGUI.TextField(rect.ShiftToRightBy(35).ShiftToLeftBy(100), GUIContent.none, namePropertyValue);
            var keyTypePropertyNewValue = (int) (BlackboardKeyType)
                EditorGUI.EnumPopup(rect.KeepToRightFor(95), GUIContent.none,
                    (BlackboardKeyType) keyTypePropertyValue);

            if (instanceSyncedPropertyValue != instanceSyncedPropertyNewValue)
            {
                instanceSyncedProperty.boolValue = instanceSyncedPropertyNewValue;
                currentSerializedObject.ApplyModifiedProperties();
            }

            if (keyTypePropertyValue != keyTypePropertyNewValue)
            {
                keyTypeProperty.enumValueIndex = keyTypePropertyNewValue;
                currentSerializedObject.ApplyModifiedProperties();
            }

            if (namePropertyValue != namePropertyNewValue)
            {
                AssetDatabase.RemoveObjectFromAsset(currentSerializedObject.targetObject);
                currentSerializedObject.targetObject.name = namePropertyNewValue;
                AssetDatabase.AddObjectToAsset(currentSerializedObject.targetObject, target);
            }
        }

        protected override void DrawHeaderCallback(Rect rect)
        {
            EditorGUI.LabelField(rect.ShiftToRightBy(10).KeepToLeftFor(30), "Sync", EditorStyles.boldLabel);
            EditorGUI.LabelField(rect.ShiftToRightBy(60).ShiftToLeftBy(75), "Name", EditorStyles.boldLabel);
            EditorGUI.LabelField(rect.KeepToRightFor(75), "Type", EditorStyles.boldLabel);
        }
    }
}