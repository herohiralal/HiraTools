using UnityEditor;

namespace HiraEditor.HiraCollection
{
	public class HiraCollectionTargetDefaultEditor : HiraCollectionTargetBaseEditor
	{
		protected override void OnInspectorGUI()
		{
			var iterator = SerializedObject.GetIterator();
			iterator.NextVisible(true);
			while (iterator.NextVisible(false))
			{
				EditorGUILayout.PropertyField(iterator);
			}
		}
	}
}