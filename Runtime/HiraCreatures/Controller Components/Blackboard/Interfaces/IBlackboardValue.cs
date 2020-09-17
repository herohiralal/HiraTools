namespace HiraCreatures.Components.Blackboard
{
    public interface IBlackboardValue
    {
        uint TypeSpecificIndex { get; }
    }

    public interface IBlackboardValue<out T> : IBlackboardValue
    {
        T TargetValue { get; }
    }
}