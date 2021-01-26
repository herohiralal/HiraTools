namespace UnityEngine
{
	public class EnumComparisonQuery<T> : ScriptableObject, IIndividualQuery where T : System.Enum
	{
		private const string has_flag = "Has Flag";
		private const string does_not_have_flag = "Does not have Flag";

		[HiraCollectionDropdown(typeof(EnumKey<>))]
		[SerializeField] protected EnumKey<T> key = null;

		[StringDropdown(false, "==", "!=", ">", ">=", "<", "<=", has_flag, does_not_have_flag)]
		[SerializeField] protected string comparisonType = "==";
		
		[SerializeField] protected T targetValue = default;

		public virtual string Condition =>
			comparisonType switch
			{
				has_flag => $"(blackboard.{key.name}.HasFlag({targetValue.ToCode()})",
				does_not_have_flag => $"!(blackboard.{key.name}.HasFlag({targetValue.ToCode()})",
				_ => $"(blackboard.{key.name} {comparisonType} {targetValue.ToCode()})"
			};
	}
}