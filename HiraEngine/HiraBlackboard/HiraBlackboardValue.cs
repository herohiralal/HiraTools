namespace Hiralal.Blackboard
{
    public abstract class HiraBlackboardValue
    {
        protected HiraBlackboardValue(uint typeSpecificIndex) => TypeSpecificIndex = typeSpecificIndex;

        public uint TypeSpecificIndex { get; }
    }

    public class HiraBlackboardValue<T> : HiraBlackboardValue
    {
        public HiraBlackboardValue(uint typeSpecificIndex, T targetValue) : base(typeSpecificIndex) => TargetValue = targetValue;

        public T TargetValue { get; }
    }
}