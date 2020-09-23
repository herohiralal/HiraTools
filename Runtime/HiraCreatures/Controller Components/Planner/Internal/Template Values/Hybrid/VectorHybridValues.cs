using UnityEngine;

namespace HiraEngine.Components.Planner.Internal
{
    public readonly struct VectorEqualsValue : IBlackboardHybridValueDefaultObject<Vector3>
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

        public IBlackboardModification ApplyTo(IReadWriteBlackboardDataSet dataSet)
        {
            var original = dataSet.Vectors[_typeSpecificIndex];
            dataSet.Vectors[_typeSpecificIndex] = _value;
            return new VectorEqualsValue(_typeSpecificIndex, original);
        }
    }
}