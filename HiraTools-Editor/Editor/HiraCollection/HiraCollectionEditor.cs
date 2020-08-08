using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

[CustomEditor(typeof(HiraCollection<>), true)]
public class HiraCollectionEditor : Editor
{
    private const string collection_property_name = "collection";
    protected SerializedProperty CollectionProperty => serializedObject.FindProperty(collection_property_name);
    protected SerializedProperty ElementAtIndex(int i) => CollectionProperty.GetArrayElementAtIndex(i);
    protected Object ObjectAtIndex(int i) => ElementAtIndex(i).objectReferenceValue;
    private Type CollectionType => (Type) target.GetType().GetProperty("CollectionType")?.GetValue(target);
    
    private ReorderableList reorderableList = null;
    private void OnEnable()
    {
        reorderableList = BuildReorderableList();
    }

    private void OnDisable()
    {
        reorderableList = null;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        reorderableList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
        EditorGUI.LabelField(EditorGUILayout.GetControlRect().KeepToRightFor(130), "Made by Rohan Jadav.");
        if (GUILayout.Button("Make Main Asset"))
            AssetDatabase.SetMainObject(target, AssetDatabase.GetAssetPath(target));
    }

    protected virtual ReorderableList BuildReorderableList()
    {
        return new ReorderableList(serializedObject,
            CollectionProperty,
            true,
            true,
            true,
            true)
        {
            drawElementCallback = DrawElementCallback,
            drawHeaderCallback = DrawHeaderCallback,
            elementHeightCallback = ElementHeightCallback,
            onAddCallback = OnAddCallback,
            onChangedCallback = OnChangedCallback,
            onRemoveCallback = OnRemoveCallback,
            onReorderCallback = OnReorderCallback,
            onReorderCallbackWithDetails = OnReorderCallbackWithDetails,
            drawNoneElementCallback = OnNoneElementCallback,
            onAddDropdownCallback = OnAddDropDownCallback,
            onCanAddCallback = OnCanAddCallback,
            onCanRemoveCallback = OnCanRemoveCallback
        };
    }

    protected virtual void DrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
    {
        EditorGUI.LabelField(rect.KeepToLeftFor(30), $"{index}.");
        
        var oldName = ObjectAtIndex(index).name;
        var newName = EditorGUI.TextField(rect.ShiftToRightBy(45).ShiftToLeftBy(100), oldName);
        if (oldName != newName)
        {
            var currentObject = ObjectAtIndex(index);
            AssetDatabase.RemoveObjectFromAsset(currentObject);
            currentObject.name = newName;
            AssetDatabase.AddObjectToAsset(currentObject, target);
        }

        EditorGUI.PropertyField(rect.KeepToRightFor(100), ElementAtIndex(index), GUIContent.none);
    }

    protected virtual void DrawHeaderCallback(Rect rect)
    {
        EditorGUI.LabelField(rect.ShiftToRightBy(15).KeepToLeftFor(30), "No.", EditorStyles.boldLabel);
        EditorGUI.LabelField(rect.ShiftToRightBy(60).ShiftToLeftBy(75), "Name", EditorStyles.boldLabel);
        EditorGUI.LabelField(rect.KeepToRightFor(75), "Reference", EditorStyles.boldLabel);
    }

    protected virtual void OnAddCallback(ReorderableList list)
    {
        serializedObject.ApplyModifiedProperties();
    }

    protected virtual void OnChangedCallback(ReorderableList list)
    {
        
    }

    protected virtual void OnRemoveCallback(ReorderableList list)
    {
        var toRemove = ObjectAtIndex(list.index);
        if (toRemove != null)
        {
            ElementAtIndex(list.index).objectReferenceValue = null;
            AssetDatabase.RemoveObjectFromAsset(toRemove);
            DestroyImmediate(toRemove);
        }
        CollectionProperty.DeleteArrayElementAtIndex(list.index);
        serializedObject.ApplyModifiedProperties();
    }

    protected virtual void OnReorderCallback(ReorderableList list)
    {
        serializedObject.ApplyModifiedProperties();
    }

    private void OnReorderCallbackWithDetails(ReorderableList list, int oldindex, int newindex)
    {
        var current = ObjectAtIndex(oldindex);

        if (oldindex < newindex)
            for (var i = oldindex; i < newindex; i++)
                ElementAtIndex(i).objectReferenceValue = ObjectAtIndex(i + 1);
        else
            for (var i = oldindex; i > newindex; i--)
                ElementAtIndex(i).objectReferenceValue = ObjectAtIndex(i - 1);

        ElementAtIndex(newindex).objectReferenceValue = current;
    }

    protected virtual void OnNoneElementCallback(Rect rect)
    {
        
    }

    protected virtual void OnAddDropDownCallback(Rect buttonRect, ReorderableList list)
    {
        var menu = new GenericMenu();
        var types = CollectionType.GetSubclasses();
        
        foreach (var type in types)
        {
            menu.AddItem(new GUIContent(type.Name), false, () =>
            {
                var so = CreateInstance(type);
                so.name = $"New{type.Name}";
                AssetDatabase.AddObjectToAsset(so, AssetDatabase.GetAssetPath(target));
                
                var newIndex = CollectionProperty.arraySize;
                CollectionProperty.InsertArrayElementAtIndex(newIndex);
                ElementAtIndex(newIndex).objectReferenceValue = so;
                serializedObject.ApplyModifiedProperties();
            });
        }
        menu.ShowAsContext();
    }

    protected virtual float ElementHeightCallback(int index) => 
        EditorGUI.GetPropertyHeight(ElementAtIndex(index));

    protected virtual bool OnCanAddCallback(ReorderableList list) => true;

    protected virtual bool OnCanRemoveCallback(ReorderableList list) => true;
}
