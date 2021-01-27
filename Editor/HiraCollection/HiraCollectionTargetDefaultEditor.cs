using UnityEditor;
using UnityEngine;

namespace HiraEditor.HiraCollection
{
	public class HiraCollectionTargetDefaultEditor : HiraCollectionTargetBaseEditor
	{
		protected override void OnInspectorGUI()
		{
			using (new EditorGUILayout.HorizontalScope())
			{
				EditorGUILayout.PrefixLabel("Object Name");
				EditorGUI.BeginChangeCheck();
				var updatedName = EditorGUILayout.TextField(GUIContent.none, Target.name);
				if (EditorGUI.EndChangeCheck())
				{
					Undo.RegisterCompleteObjectUndo(Target, $"Renamed {Target.name}");
					Target.name = updatedName;
				}
			}
			
			var iterator = SerializedObject.GetIterator();
			iterator.NextVisible(true);
			while (iterator.NextVisible(false))
			{
				EditorGUILayout.PropertyField(iterator);
			}
		}
	}
}