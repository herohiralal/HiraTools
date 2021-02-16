using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace HiraEditor.Internal
{
    public class HiraCollectionEditorRefresher
    {
        private readonly Editor _baseEditor;
        
        private SerializedObject _serializedObject;
        private IDirtiable _dirtiableAsset;

        public HiraCollectionEditorRefresher(Editor baseEditor)
        {
            _baseEditor = baseEditor;
        }
        
        public void Init(Object asset, SerializedObject serializedObject)
        {
            _dirtiableAsset = asset as IDirtiable;
            _serializedObject = serializedObject;
            
            Assert.IsNotNull(_dirtiableAsset);
        }
        
        public void OnEnable()
        {
            Undo.undoRedoPerformed += OnUndoPerformed;
        }

        public void OnDisable()
        {
            Undo.undoRedoPerformed -= OnUndoPerformed;
        }

        private void OnUndoPerformed()
        {
            _dirtiableAsset.IsDirty = true;
        
            _serializedObject.Update();
            _serializedObject.ApplyModifiedProperties();
        
            _baseEditor.Repaint();
        }

        public bool RequiresRefresh => _dirtiableAsset.IsDirty;
        public void Refreshed() => _dirtiableAsset.IsDirty = false;
    }
}