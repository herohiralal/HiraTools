namespace UnityEngine
{
	public abstract class NumericalComparisonQuery<T> : ScriptableObject, IIndividualQuery
	{
		[HiraCollectionDropdown(typeof(INumericalKey))]
		[SerializeField] protected BlackboardKey key = null;
		protected abstract string ComparisonType { get; }
		
		[SerializeField] protected T targetValue = default;
		protected abstract string TargetValue { get; }

		public virtual string Condition => $"(blackboard.{key.name} {ComparisonType} {TargetValue})";
	}
}