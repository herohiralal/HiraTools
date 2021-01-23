using UnityEngine;


namespace HiraEngine.Components.Planner.Internal
{
    public class IntPlusValue : IBlackboardModificationDefaultObject<int>
    {
        public IntPlusValue(uint typeSpecificIndex, int value) =>
            (_typeSpecificIndex, _value) = (typeSpecificIndex, value);

        private readonly uint _typeSpecificIndex;
        private readonly int _value;

        public IBlackboardModification GetNewModificationObject(uint typeSpecificIndex, int value) =>
            new IntPlusValue(typeSpecificIndex, value);

        public void ApplyTo(IReadWriteBlackboardDataSet dataSet) => dataSet.Integers[_typeSpecificIndex] += _value;

        public void ApplyTo(IBlackboardValueAccessor valueAccessor)
        {
            var value = valueAccessor.GetIntValueWithTypeSpecificIndex(_typeSpecificIndex);
            valueAccessor.SetIntValueWithTypeSpecificIndex(_typeSpecificIndex, value + _value);
        }
    }

    public class IntMultipliedByValue : IBlackboardModificationDefaultObject<int>
    {
        public IntMultipliedByValue(uint typeSpecificIndex, int value) =>
            (_typeSpecificIndex, _value) = (typeSpecificIndex, value);

        private readonly uint _typeSpecificIndex;
        private readonly int _value;
        
        public IBlackboardModification GetNewModificationObject(uint typeSpecificIndex, int value) => 
            new IntMultipliedByValue(_typeSpecificIndex, _value);

        public void ApplyTo(IReadWriteBlackboardDataSet dataSet) => dataSet.Integers[_typeSpecificIndex] *= _value;

        public void ApplyTo(IBlackboardValueAccessor valueAccessor)
        {
            var value = valueAccessor.GetIntValueWithTypeSpecificIndex(_typeSpecificIndex);
            valueAccessor.SetIntValueWithTypeSpecificIndex(_typeSpecificIndex, value * _value);
        }
    }
}