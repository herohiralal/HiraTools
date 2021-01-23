using System;
using HiraEditor.HiraCollection;
using UnityEngine;

namespace UnityEditor
{
    [CustomEditor(typeof(HiraCollection<>), true)]
    public class HiraCollectionEditor : Editor
    {
        private IHiraCollectionTargetArrayEditor[] _targetArrays;
        private HiraCollectionEditorRefresher _refresher;

        private void OnEnable()
        {
            _refresher = new HiraCollectionEditorRefresher(this);
            _refresher.Init(target, serializedObject);

            var targetTypes = TargetTypes;
            var length = targetTypes.Length;
            _targetArrays = new IHiraCollectionTargetArrayEditor[length];
            
            for (var i = 0; i < length; i++)
            {
                var editor = (IHiraCollectionTargetArrayEditor) Activator.CreateInstance(
                    typeof(HiraCollectionTargetArrayEditor<>).MakeGenericType(targetTypes[i]), new object[] {this});
                editor.Init(target, serializedObject, $"collection{i+1}");
                
                _targetArrays[i] = editor;
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
            serializedObject.Update();
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

        private Type[] TargetTypes
        {
            get
            {
                var currentType = target.GetType();

                while ((currentType = currentType.BaseType) != null)
                {
                    if (!currentType.IsGenericType) continue;
                    
                    var generic = currentType.GetGenericTypeDefinition();
                    if (generic == typeof(HiraCollection<>) || generic == typeof(HiraCollection<,>))
                    {
                        return currentType.GenericTypeArguments;
                    }
                }

                return null;
            }
        }
    }
}