namespace UnityEngine
{
	public class NumericalComparisonQuery : ScriptableObject, IIndividualQuery
	{
		[HiraCollectionDropdown(typeof(INumericalKey))]
		[SerializeField] private BlackboardKey key = null;

		[StringDropdown(false, "==", "!=", ">", ">=", "<", "<=")]
		[SerializeField] private string comparisonType = "==";
		
		[SerializeField] private float targetValue = 0;

		public string Condition => $"(blackboard.{key.name} {comparisonType} {targetValue}f)";
	}
}