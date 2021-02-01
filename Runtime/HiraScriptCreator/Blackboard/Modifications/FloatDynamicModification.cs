namespace UnityEngine
{
	public class FloatDynamicModification : ArithmeticModification
	{
		[HiraCollectionDropdown(typeof(INumericalKey))]
		[SerializeField] private BlackboardKey key = null;
		protected override BlackboardKey Key => key;
		[HiraCollectionDropdown(typeof(INumericalKey))]
		[SerializeField] private BlackboardKey delta = null;

		protected override string Value => delta.name;
		protected override string NonCodeValue => delta.name;
	}
}