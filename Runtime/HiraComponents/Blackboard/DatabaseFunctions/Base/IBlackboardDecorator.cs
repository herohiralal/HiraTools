namespace HiraEngine.Components.Blackboard.Internal
{
	public interface IBlackboardDecorator : IBlackboardDatabaseFunction<DecoratorDelegate>
	{
        bool IsValidOn(UnityEngine.IBlackboardComponent blackboard);
	}
}