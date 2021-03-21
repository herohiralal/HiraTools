using System;
using System.Collections.Generic;
using System.Linq;
using HiraEditor.Internal;
using UnityEngine;
using UnityEditor;

namespace HiraEditor
{
    [CustomEditor(typeof(HiraCollection<>), true)]
    public class HiraCollectionEditor : Editor
    {
        private IHiraCollectionTargetArrayEditor[] _targetArrays;
        private HiraCollectionEditorRefresher _refresher;
        private string[] _collectionProperties;

        private void OnEnable()
        {
            _refresher = new HiraCollectionEditorRefresher(this);
            _refresher.Init(target, serializedObject);
            _refresher.OnEnable();

            var objectType = target.GetType();
            var targetTypes = HIRA_COLLECTION_TARGET_TYPES[objectType];
            var length = targetTypes.Length;
            _targetArrays = new IHiraCollectionTargetArrayEditor[length];
            _collectionProperties = new string[length];
            
            for (var i = 0; i < length; i++)
            {
                var collectionProperty = $"collection{i + 1}";
                if (!objectType.GetData($"collection{i+1}_name", out string title)) title = "Contents";
                if (!objectType.GetData($"collection{i + 1}_required_attributes", out Type[] requiredAttributes)) requiredAttributes = null;
                if (!objectType.GetData($"collection{i + 1}_max_objects", out int maxObjects)) maxObjects = int.MaxValue;

                var creationParams = new HiraCollectionTargetArrayEditorCreationParams(this, title, requiredAttributes, maxObjects);
                
                var editor = (IHiraCollectionTargetArrayEditor) Activator.CreateInstance(
                    typeof(HiraCollectionTargetArrayEditor<>).MakeGenericType(targetTypes[i]), creationParams);
                editor.Init(target, serializedObject, collectionProperty);
                
                _targetArrays[i] = editor;
                _collectionProperties[i] = collectionProperty;
            }
        }

        private void OnDisable()
        {
            if (_targetArrays != null)
                foreach (var targetArray in _targetArrays)
                    targetArray.Clear();
            
            _refresher.OnDisable();
        }

        public override void OnInspectorGUI()
        {
            if (_targetArrays == null)
            {
                EditorGUILayout.HelpBox("An error has occured.", MessageType.Error);
                return;
            }
            
            serializedObject.Update();

            // basic properties
            var iterator = serializedObject.GetIterator();
            iterator.NextVisible(true);
            serializedObject.Update();
            while (iterator.NextVisible(false))
                if (_collectionProperties.All(s => s != iterator.name))
                    EditorGUILayout.PropertyField(iterator);
            serializedObject.ApplyModifiedProperties();

            if (_refresher.RequiresRefresh)
            {
                foreach (var targetArray in _targetArrays) 
                    targetArray.Refresh();

                _refresher.Refreshed();
            }
            
            foreach (var targetArray in _targetArrays) 
                targetArray.OnGUI();
            
            serializedObject.ApplyModifiedProperties();
        }

        public static readonly Dictionary<Type, Type[]> HIRA_COLLECTION_TARGET_TYPES;

        static HiraCollectionEditor()
        {
            HIRA_COLLECTION_TARGET_TYPES = new Dictionary<Type, Type[]>();
            var concreteHiraCollections =
                typeof(IHiraCollectionEditorInterface).GetSubclasses(false, false);
            foreach (var concreteHiraCollection in concreteHiraCollections)
                HIRA_COLLECTION_TARGET_TYPES.Add(concreteHiraCollection, GetTargetTypes(concreteHiraCollection));
        }

        private static Type[] GetTargetTypes(Type hiraCollectionType)
        {
            var currentType = hiraCollectionType;

            while ((currentType = currentType.BaseType) != null)
            {
                if (!currentType.IsGenericType) continue;

                var generic = currentType.GetGenericTypeDefinition();
                if (generic == typeof(HiraCollection<>) ||
                    generic == typeof(HiraCollection<,>) ||
                    generic == typeof(HiraCollection<,,>) ||
                    generic == typeof(HiraCollection<,,,>) ||
                    generic == typeof(HiraCollection<,,,,>))
                    return currentType.GenericTypeArguments;
            }

            return null;
        }
    }
}