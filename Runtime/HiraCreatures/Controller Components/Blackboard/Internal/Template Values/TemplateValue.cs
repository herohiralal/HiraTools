using UnityEngine;

namespace HiraCreatures.Components.Blackboard.Internal.Values
{
    public abstract class TemplateValue : IBlackboardValue
    {
        protected TemplateValue(uint typeSpecificIndex) => TypeSpecificIndex = typeSpecificIndex;
        protected uint TypeSpecificIndex { get; }
        public abstract bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet);
    }

    public abstract class TemplateValue<T> : TemplateValue, IBlackboardValueDefaultObject<T>
    {
        protected TemplateValue(uint typeSpecificIndex, T value) : base(typeSpecificIndex) => Value = value;

        protected T Value { get; }
        public abstract IBlackboardValue GetNewObject(uint typeSpecificIndex, T value);
    }
}