using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;

namespace HiraEditor.HiraCollection
{
    internal interface IHiraCollectionTargetArrayEditor
    {
        void Init(Object asset, SerializedObject serializedObject, string objectsPropertyName);
        void Clear();
        void OnGUI();
    }

    internal sealed class HiraCollectionTargetArrayEditor<T> : IHiraCollectionTargetArrayEditor where T : ScriptableObject
	{
		private HiraCollection<T> Asset { get; set; }
        
		private readonly Editor _baseEditor;

		private SerializedObject _serializedObject;
		private SerializedProperty _objectsProperty;

		private Dictionary<Type, Type> _editorTypes;
		private List<HiraCollectionTargetBaseEditor> _editors;
        
		public HiraCollectionTargetArrayEditor(Editor editor)
		{
			Assert.IsNotNull(editor);
			_baseEditor = editor;
		}

		public void Init(Object asset, SerializedObject serializedObject, string objectsPropertyName)
		{
			Asset = asset as HiraCollection<T>;
            
			Assert.IsNotNull(Asset);
			Assert.IsNotNull(serializedObject);
            
			_serializedObject = serializedObject;
			_objectsProperty = serializedObject.FindProperty(objectsPropertyName);
			Assert.IsNotNull(_objectsProperty);

			_editorTypes = new Dictionary<Type, Type>();
			_editors = new List<HiraCollectionTargetBaseEditor>();

			var editorTypes = TypeCache.GetTypesDerivedFrom(typeof(HiraCollectionTargetBaseEditor))
				.Where(t => t.IsDefined(typeof(HiraCollectionTargetEditorAttribute), false) && !t.IsAbstract);
            
			foreach (var editorType in editorTypes)
			{
				var attribute = editorType.GetCustomAttribute<HiraCollectionTargetEditorAttribute>();
				_editorTypes.Add(attribute.TargetType, editorType);
			}
            
			for (var i = 0; i < Asset.Objects.Length; i++)
				CreateEditor(Asset.Objects[i], _objectsProperty.GetArrayElementAtIndex(i));

			Undo.undoRedoPerformed += OnUndoPerformed;
		}

        private void OnUndoPerformed()
        {
            Asset.IsDirty = true;
            
            _serializedObject.Update();
            _serializedObject.ApplyModifiedProperties();
            
            _baseEditor.Repaint();
        }

        private void CreateEditor(T targetObject, SerializedProperty property, int index = -1)
        {
            var targetObjectType = targetObject.GetType();
            if (!_editorTypes.TryGetValue(targetObjectType, out var editorType))
                editorType = typeof(HiraCollectionTargetDefaultEditor);

            var editor = (HiraCollectionTargetBaseEditor) Activator.CreateInstance(editorType);
            editor.Init(targetObject, _baseEditor);
            editor.OnEnable();
            editor.BaseProperty = property.Copy();

            if (index < 0)
                _editors.Add(editor);
            else
                _editors[index] = editor;
        }

        private void RefreshEditors()
        {
            foreach (var editor in _editors)
                editor.OnDisable();

            _editors.Clear();
            
            for (var i = 0; i < Asset.Objects.Length; i++)
                CreateEditor(Asset.Objects[i], _objectsProperty.GetArrayElementAtIndex(i));
        }

        public void Clear()
        {
            if (_editors == null) return;

            foreach (var editor in _editors) 
                editor.OnDisable();

            _editors.Clear();
            _editorTypes.Clear();

            Undo.undoRedoPerformed -= OnUndoPerformed;
        }

        public void OnGUI()
        {
            if (Asset == null) return;

            if (Asset.IsDirty)
            {
                RefreshEditors();
                Asset.IsDirty = false;
            }

            var isEditable = AssetDatabase.IsOpenForEdit(Asset, StatusQueryOptions.UseCachedIfPossible);

            using (new EditorGUI.DisabledScope(!isEditable))
            {
                EditorGUILayout.LabelField("Contents".GetGUIContent(), EditorStyles.boldLabel);

                var editorsCount = _editors.Count;
                for (var i = 0; i < editorsCount; i++)
                {
                    var editor = _editors[i];
                    var id = i;

                    HiraCollectionEditorHelperLibrary.DrawSplitter();

                    Action moveUp = null;
                    if (i != 0) moveUp = () => MoveUp(id);

                    Action moveDown = null;
                    if (i != editorsCount - 1) moveDown = () => MoveDown(id);

                    if (HiraCollectionEditorHelperLibrary.DrawHeader(editor.BaseProperty, editor.SerializedObject,
                        () => ResetObject(editor.Target.GetType(), id),
                        () => RemoveObject(id),
                        moveUp,
                        moveDown,
                        ref editor.RenameMode))
                    {
                        editor.OnInternalInspectorGUI();
                    }
                }

                if (_editors.Count > 0)
                {
                    HiraCollectionEditorHelperLibrary.DrawSplitter();
                    EditorGUILayout.Space();
                }
                else
                {
                    EditorGUILayout.HelpBox("No objects in this collection.", MessageType.Info);
                }

                if (GUILayout.Button("Add", EditorStyles.miniButton))
                {
                    var menu = new GenericMenu();

                    var targetType = typeof(T);
                    if (!targetType.IsAbstract)
                    {
                        menu.AddItem(targetType.Name.GetGUIContent(), false, () => AddNewObject(targetType));
                    }
                    
                    var types = TypeCache.GetTypesDerivedFrom<T>().Where(t => !t.IsAbstract);
                    foreach (var type in types)
                    {
                        menu.AddItem(type.Name.GetGUIContent(), false, () => AddNewObject(type));
                    }
                    
                    menu.ShowAsContext();
                }

                EditorGUILayout.Space();
            }
        }

        private void AddNewObject(Type type)
        {
            _serializedObject.Update();

            var index = _objectsProperty.arraySize;

            var newObject = CreateNewObject(type, index);
            Undo.RegisterCreatedObjectUndo(newObject, $"Add {type.Name} Object");

            if (EditorUtility.IsPersistent(Asset))
                AssetDatabase.AddObjectToAsset(newObject, Asset);

            _objectsProperty.arraySize++;
            var newObjectProperty = _objectsProperty.GetArrayElementAtIndex(index);
            newObjectProperty.objectReferenceValue = newObject;

            CreateEditor(newObject, newObjectProperty);

            _serializedObject.ApplyModifiedProperties();

            if (EditorUtility.IsPersistent(Asset))
            {
                EditorUtility.SetDirty(Asset);
                AssetDatabase.SaveAssets();
            }
        }

        private void RemoveObject(int id)
        {
            var nextFoldoutState = false;
            if (id < _editors.Count - 1)
                nextFoldoutState = _editors[id + 1].BaseProperty.isExpanded;

            _editors[id].OnDisable();
            _editors.RemoveAt(id);
            
            _serializedObject.Update();

            var property = _objectsProperty.GetArrayElementAtIndex(id);
            var effect = property.objectReferenceValue;

            property.objectReferenceValue = null;

            _objectsProperty.DeleteArrayElementAtIndex(id);
            
            for (var i = 0; i < _editors.Count; i++)
                _editors[i].BaseProperty = _objectsProperty.GetArrayElementAtIndex(i).Copy();

            if (id < _editors.Count)
                _editors[id].BaseProperty.isExpanded = nextFoldoutState;

            _serializedObject.ApplyModifiedProperties();

            Undo.DestroyObjectImmediate(effect);

            EditorUtility.SetDirty(Asset);
            AssetDatabase.SaveAssets();
        }

        private void ResetObject(Type type, int id)
        {
            _editors[id].OnDisable();
            _editors[id] = null;
            
            _serializedObject.Update();

            var property = _objectsProperty.GetArrayElementAtIndex(id);
            var previousObject = property.objectReferenceValue;

            property.objectReferenceValue = null;

            var newObject = CreateNewObject(type, id);
            Undo.RegisterCreatedObjectUndo(newObject, "Reset Effect Override");

            AssetDatabase.AddObjectToAsset(newObject, Asset);

            property.objectReferenceValue = newObject;

            CreateEditor(newObject, property, id);

            _serializedObject.ApplyModifiedProperties();

            Undo.DestroyObjectImmediate(previousObject);

            EditorUtility.SetDirty(Asset);
            AssetDatabase.SaveAssets();
        }

        private T CreateNewObject(Type type, int index)
        {
            var createdObject = (T) ScriptableObject.CreateInstance(type);
            createdObject.hideFlags = HideFlags.HideInInspector | HideFlags.HideInHierarchy;
            createdObject.name = type.Name;

            if (createdObject is ICollectionAwareTarget<T> collectionAwareObject)
            {
                collectionAwareObject.ParentCollection = Asset;
                collectionAwareObject.Index = index;
            }
            
            return createdObject;
        }

        private void MoveUp(int index)
        {
            _serializedObject.Update();
        
            var current = _objectsProperty.GetArrayElementAtIndex(index);
            var previous = _objectsProperty.GetArrayElementAtIndex(index - 1);

            var (newCurrentObject, newPreviousObject) =
            (current.objectReferenceValue, previous.objectReferenceValue) =
            (previous.objectReferenceValue, current.objectReferenceValue);

            (_editors[index], _editors[index - 1]) = (_editors[index - 1], _editors[index]);

            if (newCurrentObject is ICollectionAwareTarget<T> collectionAwareObject) 
                collectionAwareObject.Index = index;

            if (newPreviousObject is ICollectionAwareTarget<T> collectionAwareObject2)
                collectionAwareObject2.Index = index - 1;

            _serializedObject.ApplyModifiedProperties();
        }

        private void MoveDown(int index)
        {
            _serializedObject.Update();
        
            var current = _objectsProperty.GetArrayElementAtIndex(index);
            var next = _objectsProperty.GetArrayElementAtIndex(index + 1);

            var (newCurrentObject, newNextObject) =
            (current.objectReferenceValue, next.objectReferenceValue) =
            (next.objectReferenceValue, current.objectReferenceValue);

            (_editors[index], _editors[index + 1]) = (_editors[index + 1], _editors[index]);

            if (newCurrentObject is ICollectionAwareTarget<T> collectionAwareObject) 
                collectionAwareObject.Index = index;

            if (newNextObject is ICollectionAwareTarget<T> collectionAwareObject2)
                collectionAwareObject2.Index = index + 1;

            _serializedObject.ApplyModifiedProperties();

            if (EditorUtility.IsPersistent(Asset))
            {
                EditorUtility.SetDirty(Asset);
                AssetDatabase.SaveAssets();
            }
        }
	}
}