using UnityEditor;

namespace HiraEditor.Internal
{
	public class HiraCollectionTargetDefaultEditor : HiraCollectionTargetBaseEditor
	{
		protected override void OnInspectorGUI()
		{
			DrawObjectNameField();
			
			var iterator = SerializedObject.GetIterator();
			iterator.NextVisible(true);
			while (iterator.NextVisible(false))
			{
				EditorGUILayout.PropertyField(iterator);
			}
		}
	}
}