using System.Globalization;

namespace UnityEngine
{
	public class FloatModification : ArithmeticModification
	{
		[HiraCollectionDropdown(typeof(FloatKey))]
		[SerializeField] private BlackboardKey key = null;
		protected override BlackboardKey Key => key;

		[SerializeField] private float delta = 0;
		protected override string Value => delta.ToCode();
		protected override string NonCodeValue => delta.ToString(CultureInfo.InvariantCulture);
	}
}