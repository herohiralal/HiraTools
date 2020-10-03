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

        public void ApplyTo(IReadWriteBlackboardDataSet dataSet) => dataSet.Floats[_typeSpecificIndex] = _value;

        public void ApplyTo(IBlackboardValueAccessor valueAccessor) => 
            valueAccessor.SetFloatValueWithTypeSpecificIndex(_typeSpecificIndex, _value);
    }
}