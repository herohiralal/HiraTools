using HiraEngine.Components.AI.LGOAP;
using UnityEditor;
using UnityEngine;

namespace HiraEditor.Internal
{
	[HiraCollectionTargetEditor(typeof(Goal))]
	public class GoalEditor : HiraCollectionTargetBaseEditor
	{
		private Goal Goal => (Goal) Target;

		protected override void OnInspectorGUI()
		{
			DrawObjectNameField();

			var openButtonRect = EditorGUILayout.GetControlRect();
			openButtonRect.x += (openButtonRect.width * 0.5f) - 50;
			openButtonRect.width = 100;
			if (GUI.Button(openButtonRect, "Open"))
				Selection.activeObject = Target;

			if (Goal.Collection1.Length > 0)
				EditorGUILayout.LabelField("Insistence-Calculators", EditorStyles.boldLabel);

			foreach (var insistenceCalculator in Goal.Collection1)
			{
				EditorGUILayout.LabelField(insistenceCalculator.ToString());
			}

			if (Goal.Collection2.Length > 0)
				EditorGUILayout.LabelField("Targets", EditorStyles.boldLabel);

			foreach (var target in Goal.Collection2)
			{
				EditorGUILayout.LabelField(target.ToString());
			}
		}
	}
}