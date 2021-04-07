using HiraEngine.Components.AI.LGOAP;
using UnityEditor;
using UnityEngine;

namespace HiraEditor.Internal
{
	[HiraCollectionTargetEditor(typeof(IntermediateGoal))]
	public class IntermediateGoalEditor : HiraCollectionTargetBaseEditor
	{
		private IntermediateGoal IntermediateGoal => (IntermediateGoal) Target;

		protected override void OnInspectorGUI()
		{
			DrawObjectNameField();

			var openButtonRect = EditorGUILayout.GetControlRect();
			openButtonRect.x += (openButtonRect.width * 0.5f) - 50;
			openButtonRect.width = 100;
			if (GUI.Button(openButtonRect, "Open"))
				Selection.activeObject = Target;

			if (IntermediateGoal.Collection1.Length > 0)
				EditorGUILayout.LabelField("Preconditions", EditorStyles.boldLabel);

			foreach (var precondition in IntermediateGoal.Collection1)
			{
				EditorGUILayout.LabelField(precondition.ToString());
			}

			if (IntermediateGoal.Collection2.Length > 0)
				EditorGUILayout.LabelField("Cost-Calculators", EditorStyles.boldLabel);

			foreach (var costCalculator in IntermediateGoal.Collection2)
			{
				EditorGUILayout.LabelField(costCalculator.ToString());
			}

			if (IntermediateGoal.Collection3.Length > 0)
				EditorGUILayout.LabelField("Effects", EditorStyles.boldLabel);

			foreach (var effects in IntermediateGoal.Collection3)
			{
				EditorGUILayout.LabelField(effects.ToString());
			}

			if (IntermediateGoal.Collection3.Length > 0)
				EditorGUILayout.LabelField("Targets", EditorStyles.boldLabel);

			foreach (var targets in IntermediateGoal.Collection3)
			{
				EditorGUILayout.LabelField(targets.ToString());
			}
		}
	}
}