using System;
using HiraEditor.HiraCollection;
using UnityEngine;

namespace UnityEditor
{
    [CustomEditor(typeof(HiraCollection<>), true)]
    public class HiraCollectionEditor : Editor
    {
        private IHiraCollectionTargetArrayEditor _targetArray;

        private void OnEnable()
        {
            _targetArray = (IHiraCollectionTargetArrayEditor) Activator.CreateInstance(
                typeof(HiraCollectionTargetArrayEditor<>).MakeGenericType(TargetType), new object[] {this});
            _targetArray.Init(target, serializedObject, "collection");
        }

        private void OnDisable()
        {
            _targetArray?.Clear();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            _targetArray.OnGUI();
            serializedObject.ApplyModifiedProperties();
        }

        private Type TargetType
        {
            get
            {
                var currentType = target.GetType();

                while ((currentType = currentType.BaseType) != null)
                {
                    if (!currentType.IsGenericType) continue;
                    
                    var generic = currentType.GetGenericTypeDefinition();
                    if (generic == typeof(HiraCollection<>))
                    {
                        return currentType.GenericTypeArguments[0];
                    }
                }

                return null;
            }
        }
    }
}