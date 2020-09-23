using UnityEngine;

namespace HiraEngine.Components.Planner.Internal
{
    public readonly struct FloatGreaterThanValue : IBlackboardQueryDefaultObject<float>
    {
        public FloatGreaterThanValue(uint typeSpecificIndex, float value)
        {
            (_typeSpecificIndex, _value) = (typeSpecificIndex, value);
        }

        private readonly uint _typeSpecificIndex;
        private readonly float _value;

        public  bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) => 
            dataSet.GetFloat(_typeSpecificIndex) > _value;

        public  IBlackboardQuery GetNewQueryObject(uint typeSpecificIndex, float value) => 
            new FloatGreaterThanValue(typeSpecificIndex, value);
    }

    public readonly struct FloatGreaterThanOrEqualToValue : IBlackboardQueryDefaultObject<float>
    {
        public FloatGreaterThanOrEqualToValue(uint typeSpecificIndex, float value)
        {
            (_typeSpecificIndex, _value) = (typeSpecificIndex, value);
        }

        private readonly uint _typeSpecificIndex;
        private readonly float _value;

        public  bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) => 
            dataSet.GetFloat(_typeSpecificIndex) >= _value;

        public  IBlackboardQuery GetNewQueryObject(uint typeSpecificIndex, float value) => 
            new FloatGreaterThanOrEqualToValue(typeSpecificIndex, value);
    }

    public readonly struct FloatLesserThanValue : IBlackboardQueryDefaultObject<float>
    {
        public FloatLesserThanValue(uint typeSpecificIndex, float value)
        {
            (_typeSpecificIndex, _value) = (typeSpecificIndex, value);
        }

        private readonly uint _typeSpecificIndex;
        private readonly float _value;

        public  bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) => 
            dataSet.GetFloat(_typeSpecificIndex) < _value;

        public  IBlackboardQuery GetNewQueryObject(uint typeSpecificIndex, float value) => 
            new FloatLesserThanValue(typeSpecificIndex, value);
    }

    public readonly struct FloatLesserThanOrEqualToValue : IBlackboardQueryDefaultObject<float>
    {
        public FloatLesserThanOrEqualToValue(uint typeSpecificIndex, float value)
        {
            (_typeSpecificIndex, _value) = (typeSpecificIndex, value);
        }

        private readonly uint _typeSpecificIndex;
        private readonly float _value;

        public  bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) => 
            dataSet.GetFloat(_typeSpecificIndex) <= _value;

        public  IBlackboardQuery GetNewQueryObject(uint typeSpecificIndex, float value) => 
            new FloatLesserThanOrEqualToValue(typeSpecificIndex, value);
    }
}