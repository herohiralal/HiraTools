using UnityEngine;

namespace HiraEngine.Components.Planner.Internal
{
    public readonly struct IntEqualsValue : IBlackboardHybridValueDefaultObject<int>
    {
        public IntEqualsValue(uint typeSpecificIndex, int value) => 
            (_typeSpecificIndex, _value) = (typeSpecificIndex, value);

        private readonly uint _typeSpecificIndex;
        private readonly int _value;

        public IBlackboardHybridValue GetNewHybridObject(uint typeSpecificIndex, int value) => 
            new IntEqualsValue(typeSpecificIndex, value);

        public bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) =>
            dataSet.GetInteger(_typeSpecificIndex) == _value;

        public IBlackboardQuery GetNewQueryObject(uint typeSpecificIndex, int value) =>
            new IntEqualsValue(typeSpecificIndex, value);

        public IBlackboardModification GetNewModificationObject(uint typeSpecificIndex, int value) => 
            new IntEqualsValue(typeSpecificIndex, value);

        public IBlackboardModification ApplyTo(IReadWriteBlackboardDataSet dataSet)
        {
            var original = dataSet.Integers[_typeSpecificIndex];
            dataSet.Integers[_typeSpecificIndex] = _value;
            return new IntEqualsValue(_typeSpecificIndex, original);
        }
    }
}