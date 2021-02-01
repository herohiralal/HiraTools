namespace UnityEngine
{
	public class EnumComparisonQuery<T> : ScriptableObject, IIndividualQuery where T : System.Enum
	{
		private const string has_flag = "Has Flag";
		private const string does_not_have_flag = "Does not have Flag";

		[HiraCollectionDropdown(typeof(EnumKey))]
		[SerializeField] protected EnumKey<T> key = null;

		[StringDropdown(false, "==", "!=", ">", ">=", "<", "<=", has_flag, does_not_have_flag)]
		[SerializeField] protected string comparisonType = "==";
		
		[SerializeField] protected T targetValue = default;
		[SerializeField] private int weight = 1;
		public int Weight => weight;

		private void OnValidate()
		{
			if (key != null) name = $"{key.name} is {targetValue.ToString().Replace(", ", " and ")}";
		}

		public virtual string Condition
		{
			get
			{
				var value = targetValue.ToCode();
				return comparisonType switch
				{
					has_flag => $"({key.name} & {value} == {value})",
					does_not_have_flag => $"({key.name} & {value} != {value})",
					_ => $"({key.name} {comparisonType} {value})"
				};
			}
		}
	}
}