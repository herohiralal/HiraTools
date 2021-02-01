namespace UnityEngine
{
	public class IntegerDynamicModification : ArithmeticModification
	{
		[HiraCollectionDropdown(typeof(INumericalKey))]
		[SerializeField] private BlackboardKey key = null;
		protected override BlackboardKey Key => key;
		[HiraCollectionDropdown(typeof(IntegerKey))]
		[SerializeField] private BlackboardKey delta = null;

		protected override string Value => delta.name;
		protected override string NonCodeValue => delta.name;
	}
}