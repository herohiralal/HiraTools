using UnityEngine;

namespace HiraEngine.Components.Planner.Internal
{
    public class BoolEqualsValue : IBlackboardHybridValueDefaultObject<bool>
    {
        public BoolEqualsValue(uint typeSpecificIndex, bool value)
        {
            (_typeSpecificIndex, _value) = (typeSpecificIndex, value);
        }

        private readonly uint _typeSpecificIndex;
        private readonly bool _value;

        public IBlackboardHybridValue GetNewHybridObject(uint typeSpecificIndex, bool value) => 
            new BoolEqualsValue(typeSpecificIndex, value);

        public bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) => 
            dataSet.GetBoolean(_typeSpecificIndex) == _value;

        public IBlackboardQuery GetNewQueryObject(uint typeSpecificIndex, bool value) => 
            new BoolEqualsValue(typeSpecificIndex, value);

        public IBlackboardModification GetNewModificationObject(uint typeSpecificIndex, bool value) => 
            new BoolEqualsValue(typeSpecificIndex, value);

        public void ApplyTo(IReadWriteBlackboardDataSet dataSet) => dataSet.Booleans[_typeSpecificIndex] = _value;

        public void ApplyTo(IBlackboardValueAccessor valueAccessor) => 
            valueAccessor.SetBooleanValueWithTypeSpecificIndex(_typeSpecificIndex, _value);
    }
}