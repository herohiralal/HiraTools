using UnityEngine;

namespace HiraEngine.Components.Planner.Internal
{
    public class FloatEqualsValue : IBlackboardHybridValueDefaultObject<float>
    {
        public FloatEqualsValue(uint typeSpecificIndex, float value) => 
            (_typeSpecificIndex, _value) = (typeSpecificIndex, value);

        private readonly uint _typeSpecificIndex;
        private readonly float _value;

        public IBlackboardHybridValue GetNewHybridObject(uint typeSpecificIndex, float value) => 
            new FloatEqualsValue(typeSpecificIndex, value);

        public bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) =>
            Mathf.Abs(dataSet.GetInteger(_typeSpecificIndex) - _value) < 0.0025f;

        public IBlackboardQuery GetNewQueryObject(uint typeSpecificIndex, float value) =>
            new FloatEqualsValue(typeSpecificIndex, value);

        public IBlackboardModification GetNewModificationObject(uint typeSpecificIndex, float value) => 
            new FloatEqualsValue(typeSpecificIndex, value);

        public IBlackboardModification ApplyTo(IReadWriteBlackboardDataSet dataSet)
        {
            var original = dataSet.Floats[_typeSpecificIndex];
            dataSet.Floats[_typeSpecificIndex] = _value;
            return new FloatEqualsValue(_typeSpecificIndex, original);
        }
    }
}