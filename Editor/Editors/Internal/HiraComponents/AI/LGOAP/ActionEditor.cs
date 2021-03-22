using HiraEngine.Components.AI.LGOAP;
using UnityEditor;
using UnityEngine;

namespace HiraEditor.Internal
{
	[HiraCollectionTargetEditor(typeof(Action))]
	public class ActionEditor : HiraCollectionTargetBaseEditor
	{
		private Action Action => (Action) Target;
		private SerializedObject _task = null;

		public override void OnEnable()
		{
			base.OnEnable();

			var executables = Action.CollectionInternal[3];
			if (executables.Length > 0)
			{
				_task = new SerializedObject(executables[0]);
			}
		}

		public override void OnDisable()
		{
			_task = null;
			
			base.OnDisable();
		}

		protected override void OnInspectorGUI()
		{
			DrawObjectNameField();

			var openButtonRect = EditorGUILayout.GetControlRect();
			openButtonRect.x += (openButtonRect.width * 0.5f) - 50;
			openButtonRect.width = 100;
			if (GUI.Button(openButtonRect, "Open"))
				Selection.activeObject = Target;

			if (Action.Collection1.Length > 0)
				EditorGUILayout.LabelField("Preconditions", EditorStyles.boldLabel);

			foreach (var precondition in Action.Collection1)
			{
				EditorGUILayout.LabelField(precondition.ToString());
			}

			if (Action.Collection2.Length > 0)
				EditorGUILayout.LabelField("Cost-Calculators", EditorStyles.boldLabel);

			foreach (var costCalculator in Action.Collection2)
			{
				EditorGUILayout.LabelField(costCalculator.ToString());
			}

			if (Action.Collection3.Length > 0)
				EditorGUILayout.LabelField("Effects", EditorStyles.boldLabel);

			foreach (var effects in Action.Collection3)
			{
				EditorGUILayout.LabelField(effects.ToString());
			}

			if (_task != null)
			{
				EditorGUILayout.LabelField("Executable", _task.targetObject.name, EditorStyles.boldLabel);
				_task.Update();
				var iterator = _task.GetIterator();
				iterator.NextVisible(true);
				while (iterator.NextVisible(false))
				{
					EditorGUILayout.PropertyField(iterator);
				}
				_task.ApplyModifiedProperties();
			}
		}
	}
}