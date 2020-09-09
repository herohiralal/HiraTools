﻿using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hiralal.HiraWorlds
{
    [CustomPropertyDrawer(typeof(HiraWorldLoader))]
    public class HiraWorldLoaderDrawer : PropertyDrawer
    {

        private const string world_property_name = "world";
        private const string load_asynchronously_property_name = "loadAsynchronously";
        private const string local_physics_mode_property_name = "localPhysicsMode";
        private const string set_active_on_load_property_name = "setActiveOnLoad";
        private const string unload_all_embedded_scene_objects_property_name = "unloadAllEmbeddedSceneObjects";

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var worldProperty = property.FindPropertyRelative(world_property_name);

            var mainRect = EditorGUI.PrefixLabel(position, label);

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            if (EditorGUI.DropdownButton(mainRect.KeepToLeftFor(20), GUIContent.none, FocusType.Passive))
                GenerateMenu(property).DropDown(mainRect.KeepToLeftFor(20));

            mainRect.x += 20;
            mainRect.width -= 20;
            EditorGUI.PropertyField(mainRect, worldProperty, GUIContent.none);

            EditorGUI.indentLevel = indent;
        }

        private static GenericMenu GenerateMenu(SerializedProperty property)
        {
            var loadAsynchronouslyProperty = property.FindPropertyRelative(load_asynchronously_property_name);
            var localPhysicsModeProperty = property.FindPropertyRelative(local_physics_mode_property_name);
            var setActiveOnLoadProperty = property.FindPropertyRelative(set_active_on_load_property_name);
            var unloadAllEmbeddedSceneObjectsProperty = property.FindPropertyRelative(unload_all_embedded_scene_objects_property_name);

            var menu = new GenericMenu();

            menu.AddItem(new GUIContent("Load Asynchronously"),
                loadAsynchronouslyProperty.boolValue,
                () =>
                {
                    loadAsynchronouslyProperty.boolValue = !loadAsynchronouslyProperty.boolValue;
                    property.serializedObject.ApplyModifiedProperties();
                });

            menu.AddSeparator("");

            menu.AddItem(new GUIContent("No Local Physics"),
                localPhysicsModeProperty.enumValueIndex == (int) LocalPhysicsMode.None,
                () =>
                {
                    localPhysicsModeProperty.intValue = 0;
                    property.serializedObject.ApplyModifiedProperties();
                });

            menu.AddItem(new GUIContent("2D Local Physics"),
                ((LocalPhysicsMode) localPhysicsModeProperty.enumValueIndex & LocalPhysicsMode.Physics2D) != 0,
                () =>
                {
                    //localPhysicsModeProperty.enumValueIndex = (int) LocalPhysicsMode.Physics2D;
                    localPhysicsModeProperty.intValue = localPhysicsModeProperty.intValue switch
                    {-1 => 2, 0 => 1, 1 => 0, 2 => -1, _ => localPhysicsModeProperty.intValue};
                    property.serializedObject.ApplyModifiedProperties();
                });

            menu.AddItem(new GUIContent("3D Local Physics"),
                ((LocalPhysicsMode) localPhysicsModeProperty.enumValueIndex & LocalPhysicsMode.Physics3D) != 0,
                () =>
                {
                    localPhysicsModeProperty.intValue = localPhysicsModeProperty.intValue switch
                    {-1 => 1, 0 => 2, 1 => -1, 2 => 0, _ => localPhysicsModeProperty.intValue};
                    property.serializedObject.ApplyModifiedProperties();
                });

            menu.AddSeparator("");

            menu.AddItem(new GUIContent("Set Active On Load"),
                setActiveOnLoadProperty.boolValue,
                () =>
                {
                    setActiveOnLoadProperty.boolValue = !setActiveOnLoadProperty.boolValue;
                    property.serializedObject.ApplyModifiedProperties();
                });

            menu.AddSeparator("");

            menu.AddItem(new GUIContent("Unload All Embedded Scene Objects"),
                unloadAllEmbeddedSceneObjectsProperty.boolValue,
                () =>
                {
                    unloadAllEmbeddedSceneObjectsProperty.boolValue = !unloadAllEmbeddedSceneObjectsProperty.boolValue;
                    property.serializedObject.ApplyModifiedProperties();
                });

            return menu;
        }
    }
}