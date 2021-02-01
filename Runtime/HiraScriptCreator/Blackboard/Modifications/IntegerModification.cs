namespace UnityEngine
{
	public class IntegerModification : ArithmeticModification
	{
		[HiraCollectionDropdown(typeof(INumericalKey))]
		[SerializeField] private BlackboardKey key = null;
		protected override BlackboardKey Key => key;

		[SerializeField] private int delta = 0;
		protected override string Value => delta.ToCode();
		protected override string NonCodeValue => delta.ToString();
	}
}