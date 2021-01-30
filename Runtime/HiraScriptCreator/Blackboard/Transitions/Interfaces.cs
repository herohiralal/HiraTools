namespace UnityEngine
{
	public interface IBlackboardGoal
	{
		string Name { get; }
		string TargetHeuristicString { get; }
	}

	public interface IBlackboardAction
	{
		string Name { get; }
		string PreconditionCheck { get; }
		string ApplyEffect { get; }
	}
}