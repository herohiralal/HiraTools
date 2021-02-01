namespace UnityEngine
{
	public abstract class ArithmeticModification : ScriptableObject, IIndividualModification
	{
		[StringDropdown(false, "+", "-", "*", "/")]
		[SerializeField] private string modification = "+";
		protected abstract BlackboardKey Key { get; }
		protected abstract string NonCodeValue { get; }
		protected abstract string Value { get; }
		public string Modification => $"{Key.name} {modification}= {Value}";

		private void OnValidate()
		{
			if (Key != null)
			{
				name = modification switch
				{
					"+" => $"Add {NonCodeValue} to {Key.name}",
					"-" => $"Subtract {NonCodeValue} from {Key.name}",
					"*" => $"Multiply {Key.name} by {NonCodeValue}",
					"/" => $"Divide {Key.name} by {NonCodeValue}",
					_ => throw new System.ArgumentOutOfRangeException()
				};
			}
		}
	}
}