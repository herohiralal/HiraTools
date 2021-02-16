using UnityEditor;
using UnityEngine;

namespace HiraEditor.Internal
{
	public abstract class HiraCollectionTargetBaseEditor
	{
		public ScriptableObject Target { get; private set; }
		public SerializedObject SerializedObject { get; private set; }
		private Editor _inspector;
		public SerializedProperty BaseProperty { get; set; }

		public void Repaint() => _inspector.Repaint();
        
		public void Init(ScriptableObject target, Editor inspector)
		{
			Target = target;
			_inspector = inspector;
			SerializedObject = new SerializedObject(target);
		}

		public virtual void OnEnable()
		{
		}

		public virtual void OnDisable()
		{
		}

		public void OnInternalInspectorGUI()
		{
			SerializedObject.Update();
			OnInspectorGUI();
			EditorGUILayout.Space();
			SerializedObject.ApplyModifiedProperties();
		}

		protected virtual void OnInspectorGUI()
		{
		}

		protected virtual void DrawObjectNameField()
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
		}
	}
}