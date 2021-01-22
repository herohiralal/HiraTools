using UnityEngine;

namespace UnityEditor
{
	public abstract class HiraCollectionTargetBaseEditor
	{
		public ScriptableObject Target { get; private set; }
		public SerializedObject SerializedObject { get; private set; }
		private Editor _inspector;
		public bool RenameMode = false;
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
	}
}