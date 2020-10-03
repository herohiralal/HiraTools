using UnityEngine;

namespace HiraEngine.Components.Planner.Internal
{
    public class StringEqualsValue : IBlackboardHybridValueDefaultObject<string>
    {
        public StringEqualsValue(uint typeSpecificIndex, string value) => 
            (_typeSpecificIndex, _value) = (typeSpecificIndex, value);

        private readonly uint _typeSpecificIndex;
        private readonly string _value;

        public IBlackboardHybridValue GetNewHybridObject(uint typeSpecificIndex, string value) => 
            new StringEqualsValue(typeSpecificIndex, value);

        public bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) =>
            dataSet.GetString(_typeSpecificIndex) == _value;

        public IBlackboardQuery GetNewQueryObject(uint typeSpecificIndex, string value) =>
            new StringEqualsValue(typeSpecificIndex, value);

        public IBlackboardModification GetNewModificationObject(uint typeSpecificIndex, string value) => 
            new StringEqualsValue(typeSpecificIndex, value);

        public void ApplyTo(IReadWriteBlackboardDataSet dataSet) => dataSet.Strings[_typeSpecificIndex] = _value;

        public void ApplyTo(IBlackboardValueAccessor valueAccessor) => 
            valueAccessor.SetStringValueWithTypeSpecificIndex(_typeSpecificIndex, _value);
    }
}