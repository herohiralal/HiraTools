using UnityEngine;


namespace HiraEngine.Components.Planner.Internal
{
    public class FloatPlusValue : IBlackboardModificationDefaultObject<float>
    {
        public FloatPlusValue(uint typeSpecificIndex, float value) =>
            (_typeSpecificIndex, _value) = (typeSpecificIndex, value);

        private readonly uint _typeSpecificIndex;
        private readonly float _value;

        public IBlackboardModification GetNewModificationObject(uint typeSpecificIndex, float value) =>
            new FloatPlusValue(typeSpecificIndex, value);

        public void ApplyTo(IReadWriteBlackboardDataSet dataSet) => dataSet.Floats[_typeSpecificIndex] += _value;

        public void ApplyTo(IBlackboardValueAccessor valueAccessor)
        {
            var value = valueAccessor.GetFloatValueWithTypeSpecificIndex(_typeSpecificIndex);
            valueAccessor.SetFloatValueWithTypeSpecificIndex(_typeSpecificIndex, value + _value);
        }
    }

    public class FloatMultipliedByValue : IBlackboardModificationDefaultObject<float>
    {
        public FloatMultipliedByValue(uint typeSpecificIndex, float value) =>
            (_typeSpecificIndex, _value) = (typeSpecificIndex, value);

        private readonly uint _typeSpecificIndex;
        private readonly float _value;
        
        public IBlackboardModification GetNewModificationObject(uint typeSpecificIndex, float value) => 
            new FloatMultipliedByValue(_typeSpecificIndex, _value);

        public void ApplyTo(IReadWriteBlackboardDataSet dataSet) => dataSet.Floats[_typeSpecificIndex] *= _value;

        public void ApplyTo(IBlackboardValueAccessor valueAccessor)
        {
            var value = valueAccessor.GetFloatValueWithTypeSpecificIndex(_typeSpecificIndex);
            valueAccessor.SetFloatValueWithTypeSpecificIndex(_typeSpecificIndex, value * _value);
        }
    }
}