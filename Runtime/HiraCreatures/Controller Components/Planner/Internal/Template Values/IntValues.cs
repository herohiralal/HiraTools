using UnityEngine;

namespace HiraEngine.Components.Planner.Internal
{
    public class IntEqualsValue : IBlackboardQueryDefaultObject<int>
    {
        public IntEqualsValue(uint typeSpecificIndex, int value)
        {
            (_typeSpecificIndex, _value) = (typeSpecificIndex, value);
        }

        private readonly uint _typeSpecificIndex;
        private readonly int _value;

        public bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) =>
            dataSet.GetInteger(_typeSpecificIndex) == _value;

        public IBlackboardQuery GetNewQueryObject(uint typeSpecificIndex, int value) =>
            new IntEqualsValue(typeSpecificIndex, value);
    }

    public class IntDoesNotEqualValue : IBlackboardQueryDefaultObject<int>
    {
        public IntDoesNotEqualValue(uint typeSpecificIndex, int value)
        {
            (_typeSpecificIndex, _value) = (typeSpecificIndex, value);
        }

        private readonly uint _typeSpecificIndex;
        private readonly int _value;

        public bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) =>
            dataSet.GetInteger(_typeSpecificIndex) != _value;

        public IBlackboardQuery GetNewQueryObject(uint typeSpecificIndex, int value) =>
            new IntDoesNotEqualValue(typeSpecificIndex, value);
    }

    public class IntGreaterThanValue : IBlackboardQueryDefaultObject<int>
    {
        public IntGreaterThanValue(uint typeSpecificIndex, int value)
        {
            (_typeSpecificIndex, _value) = (typeSpecificIndex, value);
        }

        private readonly uint _typeSpecificIndex;
        private readonly int _value;

        public bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) =>
            dataSet.GetInteger(_typeSpecificIndex) > _value;

        public IBlackboardQuery GetNewQueryObject(uint typeSpecificIndex, int value) =>
            new IntGreaterThanValue(typeSpecificIndex, value);
    }

    public class IntGreaterThanOrEqualToValue : IBlackboardQueryDefaultObject<int>
    {
        public IntGreaterThanOrEqualToValue(uint typeSpecificIndex, int value)
        {
            (_typeSpecificIndex, _value) = (typeSpecificIndex, value);
        }

        private readonly uint _typeSpecificIndex;
        private readonly int _value;

        public bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) =>
            dataSet.GetInteger(_typeSpecificIndex) >= _value;

        public IBlackboardQuery GetNewQueryObject(uint typeSpecificIndex, int value) =>
            new IntGreaterThanOrEqualToValue(typeSpecificIndex, value);
    }

    public class IntLesserThanValue : IBlackboardQueryDefaultObject<int>
    {
        public IntLesserThanValue(uint typeSpecificIndex, int value)
        {
            (_typeSpecificIndex, _value) = (typeSpecificIndex, value);
        }

        private readonly uint _typeSpecificIndex;
        private readonly int _value;

        public bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) =>
            dataSet.GetInteger(_typeSpecificIndex) < _value;

        public IBlackboardQuery GetNewQueryObject(uint typeSpecificIndex, int value) =>
            new IntLesserThanValue(typeSpecificIndex, value);
    }

    public class IntLesserThanOrEqualToValue : IBlackboardQueryDefaultObject<int>
    {
        public IntLesserThanOrEqualToValue(uint typeSpecificIndex, int value)
        {
            (_typeSpecificIndex, _value) = (typeSpecificIndex, value);
        }

        private readonly uint _typeSpecificIndex;
        private readonly int _value;

        public bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) =>
            dataSet.GetInteger(_typeSpecificIndex) <= _value;

        public IBlackboardQuery GetNewQueryObject(uint typeSpecificIndex, int value) =>
            new IntLesserThanOrEqualToValue(typeSpecificIndex, value);
    }
}