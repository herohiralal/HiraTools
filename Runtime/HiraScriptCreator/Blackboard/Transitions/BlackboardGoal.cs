namespace UnityEngine
{
	public class BlackboardGoal : ScriptableObject, IBlackboardGoal
	{
		public string Name => name;
		public string TargetHeuristicString => "0";
	}
}