namespace HiraEngine.Components.Blackboard.Internal
{
	public interface IBlackboardEffector : IBlackboardDatabaseFunction<EffectorDelegate>
	{
        void ApplyTo(UnityEngine.IBlackboardComponent blackboard);
	}
}