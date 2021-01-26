namespace UnityEngine
{
	public class IntegerComparisonQuery : NumericalComparisonQuery<int>
	{
		[StringDropdown(false, "==", "!=", ">", ">=", "<", "<=")]
		[SerializeField] protected string comparisonType = "==";

		protected override string ComparisonType => comparisonType;
		protected override string TargetValue => targetValue.ToCode();
	}
}