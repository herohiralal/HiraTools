using UnityEngine;

namespace HiraEngine.Components.Planner.Internal
{
    public class FloatGreaterThanValue : IBlackboardQueryDefaultObject<float>
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

    public class FloatGreaterThanOrEqualToValue : IBlackboardQueryDefaultObject<float>
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

    public class FloatLesserThanValue : IBlackboardQueryDefaultObject<float>
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

    public class FloatLesserThanOrEqualToValue : IBlackboardQueryDefaultObject<float>
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