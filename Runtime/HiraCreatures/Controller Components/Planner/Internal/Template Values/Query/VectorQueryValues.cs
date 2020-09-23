using UnityEngine;

namespace HiraEngine.Components.Planner.Internal
{
    public readonly struct VectorDoesNotEqualValue : IBlackboardQueryDefaultObject<Vector3>
    {
        public VectorDoesNotEqualValue(uint typeSpecificIndex, Vector3 value)
        {
            (_typeSpecificIndex, _value) = (typeSpecificIndex, value);
        }

        private readonly uint _typeSpecificIndex;
        private readonly Vector3 _value;

        public bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) =>
            dataSet.GetVector(_typeSpecificIndex) != _value;

        public IBlackboardQuery GetNewQueryObject(uint typeSpecificIndex, Vector3 value) =>
            new VectorDoesNotEqualValue(typeSpecificIndex, value);
    }

    public readonly struct VectorIsParallelToValue : IBlackboardQueryDefaultObject<Vector3>
    {
        public VectorIsParallelToValue(uint typeSpecificIndex, Vector3 value)
        {
            (_typeSpecificIndex, _value) = (typeSpecificIndex, value.normalized);
        }

        private readonly uint _typeSpecificIndex;
        private readonly Vector3 _value;

        public bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) =>
            Vector3.Dot(dataSet.GetVector(_typeSpecificIndex).normalized, _value) > 0.98f;

        public IBlackboardQuery GetNewQueryObject(uint typeSpecificIndex, Vector3 value) =>
            new VectorIsParallelToValue(typeSpecificIndex, _value);
    }

    public readonly struct VectorIsNotParallelToValue : IBlackboardQueryDefaultObject<Vector3>
    {
        public VectorIsNotParallelToValue(uint typeSpecificIndex, Vector3 value)
        {
            (_typeSpecificIndex, _value) = (typeSpecificIndex, value.normalized);
        }

        private readonly uint _typeSpecificIndex;
        private readonly Vector3 _value;

        public bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) =>
            Vector3.Dot(dataSet.GetVector(_typeSpecificIndex).normalized, _value) <= 0.98f;

        public IBlackboardQuery GetNewQueryObject(uint typeSpecificIndex, Vector3 value) =>
            new VectorIsNotParallelToValue(typeSpecificIndex, value);
    }

    public readonly struct VectorIsPerpendicularToValue : IBlackboardQueryDefaultObject<Vector3>
    {
        public VectorIsPerpendicularToValue(uint typeSpecificIndex, Vector3 value)
        {
            (_typeSpecificIndex, _value) = (typeSpecificIndex, value.normalized);
        }

        private readonly uint _typeSpecificIndex;
        private readonly Vector3 _value;

        public bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) =>
            Mathf.Abs(Vector3.Dot(dataSet.GetVector(_typeSpecificIndex).normalized, _value)) < 0.02f;

        public IBlackboardQuery GetNewQueryObject(uint typeSpecificIndex, Vector3 value) =>
            new VectorIsPerpendicularToValue(typeSpecificIndex, value);
    }

    public readonly struct VectorIsNotPerpendicularToValue : IBlackboardQueryDefaultObject<Vector3>
    {
        public VectorIsNotPerpendicularToValue(uint typeSpecificIndex, Vector3 value)
        {
            (_typeSpecificIndex, _value) = (typeSpecificIndex, value.normalized);
        }

        private readonly uint _typeSpecificIndex;
        private readonly Vector3 _value;

        public bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) =>
            Mathf.Abs(Vector3.Dot(dataSet.GetVector(_typeSpecificIndex).normalized, _value)) >= 0.02f;

        public IBlackboardQuery GetNewQueryObject(uint typeSpecificIndex, Vector3 value) =>
            new VectorIsNotPerpendicularToValue(typeSpecificIndex, value);
    }

    public readonly struct VectorIsAntiParallelToValue : IBlackboardQueryDefaultObject<Vector3>
    {
        public VectorIsAntiParallelToValue(uint typeSpecificIndex, Vector3 value)
        {
            (_typeSpecificIndex, _value) = (typeSpecificIndex, value.normalized);
        }

        private readonly uint _typeSpecificIndex;
        private readonly Vector3 _value;

        public bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) =>
            Vector3.Dot(dataSet.GetVector(_typeSpecificIndex).normalized, _value) < -0.98f;

        public IBlackboardQuery GetNewQueryObject(uint typeSpecificIndex, Vector3 value) =>
            new VectorIsAntiParallelToValue(typeSpecificIndex, value);
    }

    public readonly struct VectorIsNotAntiParallelToValue : IBlackboardQueryDefaultObject<Vector3>
    {
        public VectorIsNotAntiParallelToValue(uint typeSpecificIndex, Vector3 value)
        {
            (_typeSpecificIndex, _value) = (typeSpecificIndex, value.normalized);
        }

        private readonly uint _typeSpecificIndex;
        private readonly Vector3 _value;

        public bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) =>
            Vector3.Dot(dataSet.GetVector(_typeSpecificIndex).normalized, _value) >= -0.98f;

        public IBlackboardQuery GetNewQueryObject(uint typeSpecificIndex, Vector3 value) =>
            new VectorIsNotAntiParallelToValue(typeSpecificIndex, value);
    }

    public readonly struct VectorHasAHigherMagnitudeThanValue : IBlackboardQueryDefaultObject<Vector3>
    {
        public VectorHasAHigherMagnitudeThanValue(uint typeSpecificIndex, Vector3 value)
        {
            (_typeSpecificIndex, _value) = (typeSpecificIndex, value.sqrMagnitude);
        }

        private readonly uint _typeSpecificIndex;
        private readonly float _value;

        public bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) =>
            dataSet.GetVector(_typeSpecificIndex).sqrMagnitude > _value;

        public IBlackboardQuery GetNewQueryObject(uint typeSpecificIndex, Vector3 value) =>
            new VectorHasAHigherMagnitudeThanValue(typeSpecificIndex, value);
    }

    public readonly struct VectorHasALowerMagnitudeThanValue : IBlackboardQueryDefaultObject<Vector3>
    {
        public VectorHasALowerMagnitudeThanValue(uint typeSpecificIndex, Vector3 value)
        {
            (_typeSpecificIndex, _value) = (typeSpecificIndex, value.sqrMagnitude);
        }

        private readonly uint _typeSpecificIndex;
        private readonly float _value;

        public bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) =>
            dataSet.GetVector(_typeSpecificIndex).sqrMagnitude < _value;

        public IBlackboardQuery GetNewQueryObject(uint typeSpecificIndex, Vector3 value) =>
            new VectorHasALowerMagnitudeThanValue(typeSpecificIndex, value);
    }

    public readonly struct VectorHasTheSameMagnitudeAsValue : IBlackboardQueryDefaultObject<Vector3>
    {
        public VectorHasTheSameMagnitudeAsValue(uint typeSpecificIndex, Vector3 value)
        {
            (_typeSpecificIndex, _value) = (typeSpecificIndex, value.sqrMagnitude);
        }

        private readonly uint _typeSpecificIndex;
        private readonly float _value;

        public bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) =>
            Mathf.Abs(dataSet.GetVector(_typeSpecificIndex).sqrMagnitude - _value) < 0.0025f;

        public IBlackboardQuery GetNewQueryObject(uint typeSpecificIndex, Vector3 value) =>
            new VectorHasTheSameMagnitudeAsValue(typeSpecificIndex, value);
    }

    public readonly struct VectorDoesNotHaveTheSameMagnitudeAsValue : IBlackboardQueryDefaultObject<Vector3>
    {
        public VectorDoesNotHaveTheSameMagnitudeAsValue(uint typeSpecificIndex, Vector3 value)
        {
            (_typeSpecificIndex, _value) = (typeSpecificIndex, value.sqrMagnitude);
        }

        private readonly uint _typeSpecificIndex;
        private readonly float _value;

        public bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) =>
            Mathf.Abs(dataSet.GetVector(_typeSpecificIndex).sqrMagnitude - _value) > 0.0025f;

        public IBlackboardQuery GetNewQueryObject(uint typeSpecificIndex, Vector3 value) =>
            new VectorDoesNotHaveTheSameMagnitudeAsValue(typeSpecificIndex, value);
    }
}