using UnityEngine;

namespace HiraEngine.Components.Planner.Internal
{
    public class VectorEqualsValue : IBlackboardHybridValueDefaultObject<Vector3>
    {
        public VectorEqualsValue(uint typeSpecificIndex, Vector3 value) => 
            (_typeSpecificIndex, _value) = (typeSpecificIndex, value);

        private readonly uint _typeSpecificIndex;
        private readonly Vector3 _value;

        public IBlackboardHybridValue GetNewHybridObject(uint typeSpecificIndex, Vector3 value) => 
            new VectorEqualsValue(typeSpecificIndex, value);

        public bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) =>
            dataSet.GetVector(_typeSpecificIndex) == _value;

        public IBlackboardQuery GetNewQueryObject(uint typeSpecificIndex, Vector3 value) =>
            new VectorEqualsValue(typeSpecificIndex, value);

        public IBlackboardModification GetNewModificationObject(uint typeSpecificIndex, Vector3 value) => 
            new VectorEqualsValue(typeSpecificIndex, value);

        public void ApplyTo(IReadWriteBlackboardDataSet dataSet) => dataSet.Vectors[_typeSpecificIndex] = _value;

        public void ApplyTo(IBlackboardValueAccessor valueAccessor) => 
            valueAccessor.SetVectorValueWithTypeSpecificIndex(_typeSpecificIndex, _value);
    }
}