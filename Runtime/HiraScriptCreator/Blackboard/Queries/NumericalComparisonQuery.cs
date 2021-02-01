namespace UnityEngine
{
	public abstract class NumericalComparisonQuery<T> : ScriptableObject, IIndividualQuery
	{
		[SerializeField] private int weight = 1;
		public int Weight => weight;
		[HiraCollectionDropdown(typeof(INumericalKey))]
		[SerializeField] protected BlackboardKey key = null;
		protected abstract string ComparisonType { get; }
		
		[SerializeField] protected T targetValue = default;
		protected abstract string TargetValue { get; }

		private void OnValidate()
		{
			if (key != null) name = $"{key.name} is {targetValue}";
		}

		public virtual string Condition => $"({key.name} {ComparisonType} {TargetValue})";
	}
}