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
			if (key != null)
			{
				var comparison = ComparisonType switch
				{
					"==" => "",
					"!=" => "not ",
					">" => "greater than ",
					"<" => "lesser than ",
					"<=" => "lesser than or equal to ",
					">=" => "greater than or equal to ",
					_ => throw new System.ArgumentOutOfRangeException()
				};

				name = $"{key.name} is {comparison}{targetValue}";
			}
		}

		public virtual string Condition => $"({key.name} {ComparisonType} {TargetValue})";
	}
}