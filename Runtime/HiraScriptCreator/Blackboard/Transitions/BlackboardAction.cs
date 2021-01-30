namespace UnityEngine
{
	public class BlackboardAction : ScriptableObject, IBlackboardAction
	{
		public string Name => name;
		public string PreconditionCheck => "true";
		public string ApplyEffect => "";
	}
}