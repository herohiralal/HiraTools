using UnityEngine;

namespace HiraEngine.Components.Planner.Internal
{
    public class StringEqualsValue :  IBlackboardQueryDefaultObject<string>
    {
        public StringEqualsValue(uint typeSpecificIndex, string value)
        {
            (_typeSpecificIndex, _value) = (typeSpecificIndex, value);
        }

        private readonly uint _typeSpecificIndex;
        private readonly string _value;

        public bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) =>
            dataSet.GetString(_typeSpecificIndex)== _value;

        public IBlackboardQuery GetNewQueryObject(uint typeSpecificIndex, string value) =>
            new StringEqualsValue(typeSpecificIndex, value);
    }

    public class StringDoesNotEqualValue :  IBlackboardQueryDefaultObject<string>
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

    public class StringContainsValue :  IBlackboardQueryDefaultObject<string>
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

    public class StringContainedByValue :  IBlackboardQueryDefaultObject<string>
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

    public class StringDoesNotContainValue :  IBlackboardQueryDefaultObject<string>
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

    public class StringNotContainedByValue :  IBlackboardQueryDefaultObject<string>
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

    public class StringStartsWithValue :  IBlackboardQueryDefaultObject<string>
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

    public class StringDoesNotStartWithValue :  IBlackboardQueryDefaultObject<string>
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

    public class StringEndsWithValue :  IBlackboardQueryDefaultObject<string>
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

    public class StringDoesNotEndWithValue :  IBlackboardQueryDefaultObject<string>
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