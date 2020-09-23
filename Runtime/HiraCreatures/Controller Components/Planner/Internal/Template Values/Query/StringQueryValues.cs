using UnityEngine;

namespace HiraEngine.Components.Planner.Internal
{
    public readonly struct StringDoesNotEqualValue :  IBlackboardQueryDefaultObject<string>
    {
        public StringDoesNotEqualValue(uint typeSpecificIndex, string value)
        {
            (_typeSpecificIndex, _value) = (typeSpecificIndex, value);
        }

        private readonly uint _typeSpecificIndex;
        private readonly string _value;

        public bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) => 
            dataSet.GetString(_typeSpecificIndex) != _value;

        public IBlackboardQuery GetNewQueryObject(uint typeSpecificIndex, string value) => 
            new StringDoesNotEqualValue(typeSpecificIndex, value);
    }

    public readonly struct StringContainsValue :  IBlackboardQueryDefaultObject<string>
    {
        public StringContainsValue(uint typeSpecificIndex, string value)
        {
            (_typeSpecificIndex, _value) = (typeSpecificIndex, value);
        }

        private readonly uint _typeSpecificIndex;
        private readonly string _value;

        public bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) =>
            dataSet.GetString(_typeSpecificIndex).Contains(_value);

        public IBlackboardQuery GetNewQueryObject(uint typeSpecificIndex, string value) =>
            new StringContainsValue(typeSpecificIndex, value);
    }

    public readonly struct StringContainedByValue :  IBlackboardQueryDefaultObject<string>
    {
        public StringContainedByValue(uint typeSpecificIndex, string value)
        {
            (_typeSpecificIndex, _value) = (typeSpecificIndex, value);
        }

        private readonly uint _typeSpecificIndex;
        private readonly string _value;

        public bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) =>
            _value.Contains(dataSet.GetString(_typeSpecificIndex));

        public IBlackboardQuery GetNewQueryObject(uint typeSpecificIndex, string value) =>
            new StringContainedByValue(typeSpecificIndex, value);
    }

    public readonly struct StringDoesNotContainValue :  IBlackboardQueryDefaultObject<string>
    {
        public StringDoesNotContainValue(uint typeSpecificIndex, string value)
        {
            (_typeSpecificIndex, _value) = (typeSpecificIndex, value);
        }

        private readonly uint _typeSpecificIndex;
        private readonly string _value;

        public bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) =>
            !dataSet.GetString(_typeSpecificIndex).Contains(_value);

        public IBlackboardQuery GetNewQueryObject(uint typeSpecificIndex, string value) =>
            new StringDoesNotContainValue(typeSpecificIndex, value);
    }

    public readonly struct StringNotContainedByValue :  IBlackboardQueryDefaultObject<string>
    {
        public StringNotContainedByValue(uint typeSpecificIndex, string value)
        {
            (_typeSpecificIndex, _value) = (typeSpecificIndex, value);
        }

        private readonly uint _typeSpecificIndex;
        private readonly string _value;

        public bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) =>
            !_value.Contains(dataSet.GetString(_typeSpecificIndex));

        public IBlackboardQuery GetNewQueryObject(uint typeSpecificIndex, string value) =>
            new StringNotContainedByValue(typeSpecificIndex, value);
    }

    public readonly struct StringStartsWithValue :  IBlackboardQueryDefaultObject<string>
    {
        public StringStartsWithValue(uint typeSpecificIndex, string value)
        {
            (_typeSpecificIndex, _value) = (typeSpecificIndex, value);
        }

        private readonly uint _typeSpecificIndex;
        private readonly string _value;

        public bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) =>
            dataSet.GetString(_typeSpecificIndex).StartsWith(_value);

        public IBlackboardQuery GetNewQueryObject(uint typeSpecificIndex, string value) =>
            new StringStartsWithValue(typeSpecificIndex, value);
    }

    public readonly struct StringDoesNotStartWithValue :  IBlackboardQueryDefaultObject<string>
    {
        public StringDoesNotStartWithValue(uint typeSpecificIndex, string value)
        {
            (_typeSpecificIndex, _value) = (typeSpecificIndex, value);
        }

        private readonly uint _typeSpecificIndex;
        private readonly string _value;

        public bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) =>
            !dataSet.GetString(_typeSpecificIndex).StartsWith(_value);

        public IBlackboardQuery GetNewQueryObject(uint typeSpecificIndex, string value) =>
            new StringDoesNotStartWithValue(typeSpecificIndex, value);
    }

    public readonly struct StringEndsWithValue :  IBlackboardQueryDefaultObject<string>
    {
        public StringEndsWithValue(uint typeSpecificIndex, string value)
        {
            (_typeSpecificIndex, _value) = (typeSpecificIndex, value);
        }

        private readonly uint _typeSpecificIndex;
        private readonly string _value;

        public bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) =>
            dataSet.GetString(_typeSpecificIndex).EndsWith(_value);

        public IBlackboardQuery GetNewQueryObject(uint typeSpecificIndex, string value) =>
            new StringEndsWithValue(typeSpecificIndex, value);
    }

    public readonly struct StringDoesNotEndWithValue :  IBlackboardQueryDefaultObject<string>
    {
        public StringDoesNotEndWithValue(uint typeSpecificIndex, string value)
        {
            (_typeSpecificIndex, _value) = (typeSpecificIndex, value);
        }

        private readonly uint _typeSpecificIndex;
        private readonly string _value;

        public bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) =>
            !dataSet.GetString(_typeSpecificIndex).EndsWith(_value);

        public IBlackboardQuery GetNewQueryObject(uint typeSpecificIndex, string value) =>
            new StringDoesNotEndWithValue(typeSpecificIndex, value);
    }
}