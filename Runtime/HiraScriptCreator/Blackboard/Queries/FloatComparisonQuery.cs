namespace UnityEngine
{
	public class FloatComparisonQuery : NumericalComparisonQuery<float>
	{
		[StringDropdown(false, ">", ">=", "<", "<=")]
		[SerializeField] protected string comparisonType = ">";

		protected override string ComparisonType => comparisonType;
		
		protected override string TargetValue => targetValue.ToCode();
	}
}