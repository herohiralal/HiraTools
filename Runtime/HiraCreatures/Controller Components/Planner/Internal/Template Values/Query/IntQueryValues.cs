using UnityEngine;

namespace HiraEngine.Components.Planner.Internal
{
    public readonly struct IntDoesNotEqualValue : IBlackboardQueryDefaultObject<int>
    {
        public IntDoesNotEqualValue(uint typeSpecificIndex, int value) => 
            (_typeSpecificIndex, _value) = (typeSpecificIndex, value);

        private readonly uint _typeSpecificIndex;
        private readonly int _value;

        public bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) =>
            dataSet.GetInteger(_typeSpecificIndex) != _value;

        public IBlackboardQuery GetNewQueryObject(uint typeSpecificIndex, int value) =>
            new IntDoesNotEqualValue(typeSpecificIndex, value);
    }

    public readonly struct IntGreaterThanValue : IBlackboardQueryDefaultObject<int>
    {
        public IntGreaterThanValue(uint typeSpecificIndex, int value) => 
            (_typeSpecificIndex, _value) = (typeSpecificIndex, value);

        private readonly uint _typeSpecificIndex;
        private readonly int _value;

        public bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) =>
            dataSet.GetInteger(_typeSpecificIndex) > _value;

        public IBlackboardQuery GetNewQueryObject(uint typeSpecificIndex, int value) =>
            new IntGreaterThanValue(typeSpecificIndex, value);
    }

    public readonly struct IntGreaterThanOrEqualToValue : IBlackboardQueryDefaultObject<int>
    {
        public IntGreaterThanOrEqualToValue(uint typeSpecificIndex, int value) => 
            (_typeSpecificIndex, _value) = (typeSpecificIndex, value);

        private readonly uint _typeSpecificIndex;
        private readonly int _value;

        public bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) =>
            dataSet.GetInteger(_typeSpecificIndex) >= _value;

        public IBlackboardQuery GetNewQueryObject(uint typeSpecificIndex, int value) =>
            new IntGreaterThanOrEqualToValue(typeSpecificIndex, value);
    }

    public readonly struct IntLesserThanValue : IBlackboardQueryDefaultObject<int>
    {
        public IntLesserThanValue(uint typeSpecificIndex, int value) => 
            (_typeSpecificIndex, _value) = (typeSpecificIndex, value);

        private readonly uint _typeSpecificIndex;
        private readonly int _value;

        public bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) =>
            dataSet.GetInteger(_typeSpecificIndex) < _value;

        public IBlackboardQuery GetNewQueryObject(uint typeSpecificIndex, int value) =>
            new IntLesserThanValue(typeSpecificIndex, value);
    }

    public readonly struct IntLesserThanOrEqualToValue : IBlackboardQueryDefaultObject<int>
    {
        public IntLesserThanOrEqualToValue(uint typeSpecificIndex, int value) => 
            (_typeSpecificIndex, _value) = (typeSpecificIndex, value);

        private readonly uint _typeSpecificIndex;
        private readonly int _value;

        public bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) =>
            dataSet.GetInteger(_typeSpecificIndex) <= _value;

        public IBlackboardQuery GetNewQueryObject(uint typeSpecificIndex, int value) =>
            new IntLesserThanOrEqualToValue(typeSpecificIndex, value);
    }
}