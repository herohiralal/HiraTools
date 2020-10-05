using UnityEngine;

namespace HiraEngine.Components.Planner.Internal
{
    public class IntEqualsValue : IBlackboardHybridValueDefaultObject<int>
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

        public void ApplyTo(IReadWriteBlackboardDataSet dataSet) =>
            dataSet.Integers[_typeSpecificIndex] = _value;

        public void ApplyTo(IBlackboardValueAccessor valueAccessor) =>
            valueAccessor.SetIntValueWithTypeSpecificIndex(_typeSpecificIndex, _value);
    }

    public class IntHasFlagValue : IBlackboardHybridValueDefaultObject<int>
    {
        public IntHasFlagValue(uint typeSpecificIndex, int value) =>
            (_typeSpecificIndex, _value) = (typeSpecificIndex, value);

        private readonly uint _typeSpecificIndex;
        private readonly int _value;

        public IBlackboardHybridValue GetNewHybridObject(uint typeSpecificIndex, int value) =>
            new IntHasFlagValue(typeSpecificIndex, value);

        public bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) =>
            (dataSet.GetInteger(_typeSpecificIndex) & _value) != 0;

        public IBlackboardQuery GetNewQueryObject(uint typeSpecificIndex, int value) =>
            new IntHasFlagValue(typeSpecificIndex, value);

        public IBlackboardModification GetNewModificationObject(uint typeSpecificIndex, int value) =>
            new IntHasFlagValue(typeSpecificIndex, value);

        public void ApplyTo(IReadWriteBlackboardDataSet dataSet) =>
            dataSet.Integers[_typeSpecificIndex] |= _value;

        public void ApplyTo(IBlackboardValueAccessor valueAccessor) =>
            valueAccessor.SetIntValueWithTypeSpecificIndex(_typeSpecificIndex,
                valueAccessor.GetIntValueWithTypeSpecificIndex(_typeSpecificIndex) | _value);
    }
    
    public class IntDoesNotHaveFlagValue : IBlackboardHybridValueDefaultObject<int>
    {
        public IntDoesNotHaveFlagValue(uint typeSpecificIndex, int value) =>
            (_typeSpecificIndex, _value) = (typeSpecificIndex, value);

        private readonly uint _typeSpecificIndex;
        private readonly int _value;

        public IBlackboardHybridValue GetNewHybridObject(uint typeSpecificIndex, int value) =>
            new IntDoesNotHaveFlagValue(typeSpecificIndex, value);

        public bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) =>
            (dataSet.GetInteger(_typeSpecificIndex) & _value) == 0;

        public IBlackboardQuery GetNewQueryObject(uint typeSpecificIndex, int value) =>
            new IntDoesNotHaveFlagValue(typeSpecificIndex, value);

        public IBlackboardModification GetNewModificationObject(uint typeSpecificIndex, int value) =>
            new IntDoesNotHaveFlagValue(typeSpecificIndex, value);

        public void ApplyTo(IReadWriteBlackboardDataSet dataSet) =>
            dataSet.Integers[_typeSpecificIndex] &= ~_value;

        public void ApplyTo(IBlackboardValueAccessor valueAccessor) =>
            valueAccessor.SetIntValueWithTypeSpecificIndex(_typeSpecificIndex,
                valueAccessor.GetIntValueWithTypeSpecificIndex(_typeSpecificIndex) & ~_value);
    }
}