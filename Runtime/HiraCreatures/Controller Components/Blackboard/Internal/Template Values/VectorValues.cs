using UnityEngine;

namespace HiraCreatures.Components.Blackboard.Internal.Values
{
    public class VectorEqualsValue : TemplateValue<Vector3>
    {
        public VectorEqualsValue(uint typeSpecificIndex, Vector3 value) : base(typeSpecificIndex, value)
        {
        }

        public override bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) => 
            dataSet.GetVector(TypeSpecificIndex) == Value;

        public override IBlackboardValue GetNewObject(uint typeSpecificIndex, Vector3 value) => 
            new VectorEqualsValue(typeSpecificIndex, value);
    }

    public class VectorDoesNotEqualValue : TemplateValue<Vector3>
    {
        public VectorDoesNotEqualValue(uint typeSpecificIndex, Vector3 value) : base(typeSpecificIndex, value)
        {
        }

        public override bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) => 
            dataSet.GetVector(TypeSpecificIndex) != Value;

        public override IBlackboardValue GetNewObject(uint typeSpecificIndex, Vector3 value) => 
            new VectorDoesNotEqualValue(typeSpecificIndex, value);
    }

    public class VectorIsParallelToValue : TemplateValue<Vector3>
    {
        public VectorIsParallelToValue(uint typeSpecificIndex, Vector3 value) : base(typeSpecificIndex, value.normalized)
        {
        }

        public override bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) => 
            Vector3.Dot(dataSet.GetVector(TypeSpecificIndex).normalized, Value) > 0.98f;

        public override IBlackboardValue GetNewObject(uint typeSpecificIndex, Vector3 value) => 
            new VectorIsParallelToValue(typeSpecificIndex, value);
    }

    public class VectorIsNotParallelToValue : TemplateValue<Vector3>
    {
        public VectorIsNotParallelToValue(uint typeSpecificIndex, Vector3 value) : base(typeSpecificIndex, value.normalized)
        {
        }

        public override bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) => 
            Vector3.Dot(dataSet.GetVector(TypeSpecificIndex).normalized, Value) <= 0.98f;

        public override IBlackboardValue GetNewObject(uint typeSpecificIndex, Vector3 value) => 
            new VectorIsNotParallelToValue(typeSpecificIndex, value);
    }

    public class VectorIsPerpendicularToValue : TemplateValue<Vector3>
    {
        public VectorIsPerpendicularToValue(uint typeSpecificIndex, Vector3 value) : base(typeSpecificIndex, value.normalized)
        {
        }

        public override bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) => 
            Mathf.Abs(Vector3.Dot(dataSet.GetVector(TypeSpecificIndex).normalized, Value)) < 0.02f;

        public override IBlackboardValue GetNewObject(uint typeSpecificIndex, Vector3 value) => 
            new VectorIsPerpendicularToValue(typeSpecificIndex, value);
    }

    public class VectorIsNotPerpendicularToValue : TemplateValue<Vector3>
    {
        public VectorIsNotPerpendicularToValue(uint typeSpecificIndex, Vector3 value) : base(typeSpecificIndex, value.normalized)
        {
        }

        public override bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) => 
            Mathf.Abs(Vector3.Dot(dataSet.GetVector(TypeSpecificIndex).normalized, Value)) >= 0.02f;

        public override IBlackboardValue GetNewObject(uint typeSpecificIndex, Vector3 value) => 
            new VectorIsNotPerpendicularToValue(typeSpecificIndex, value);
    }

    public class VectorIsAntiParallelToValue : TemplateValue<Vector3>
    {
        public VectorIsAntiParallelToValue(uint typeSpecificIndex, Vector3 value) : base(typeSpecificIndex, value.normalized)
        {
        }

        public override bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) => 
            Vector3.Dot(dataSet.GetVector(TypeSpecificIndex).normalized, Value) < -0.98f;

        public override IBlackboardValue GetNewObject(uint typeSpecificIndex, Vector3 value) => 
            new VectorIsAntiParallelToValue(typeSpecificIndex, value);
    }

    public class VectorIsNotAntiParallelToValue : TemplateValue<Vector3>
    {
        public VectorIsNotAntiParallelToValue(uint typeSpecificIndex, Vector3 value) : base(typeSpecificIndex, value.normalized)
        {
        }

        public override bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) => 
            Vector3.Dot(dataSet.GetVector(TypeSpecificIndex).normalized, Value) >= -0.98f;

        public override IBlackboardValue GetNewObject(uint typeSpecificIndex, Vector3 value) => 
            new VectorIsNotAntiParallelToValue(typeSpecificIndex, value);
    }

    public class VectorHasAHigherMagnitudeThanValue : TemplateValue<Vector3>
    {
        public VectorHasAHigherMagnitudeThanValue(uint typeSpecificIndex, Vector3 value) : base(typeSpecificIndex, value) => 
            Value = value.sqrMagnitude;

        private new float Value { get; }

        public override bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) => 
            dataSet.GetVector(TypeSpecificIndex).sqrMagnitude > Value;

        public override IBlackboardValue GetNewObject(uint typeSpecificIndex, Vector3 value) => 
            new VectorHasAHigherMagnitudeThanValue(typeSpecificIndex, value);
    }

    public class VectorHasALowerMagnitudeThanValue : TemplateValue<Vector3>
    {
        public VectorHasALowerMagnitudeThanValue(uint typeSpecificIndex, Vector3 value) : base(typeSpecificIndex, value) => 
            Value = value.sqrMagnitude;

        private new float Value { get; }

        public override bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) => 
            dataSet.GetVector(TypeSpecificIndex).sqrMagnitude < Value;

        public override IBlackboardValue GetNewObject(uint typeSpecificIndex, Vector3 value) => 
            new VectorHasALowerMagnitudeThanValue(typeSpecificIndex, value);
    }

    public class VectorHasTheSameMagnitudeAsValue : TemplateValue<Vector3>
    {
        public VectorHasTheSameMagnitudeAsValue(uint typeSpecificIndex, Vector3 value) : base(typeSpecificIndex, value) => 
            Value = value.sqrMagnitude;

        private new float Value { get; }

        public override bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) => 
            Mathf.Abs(dataSet.GetVector(TypeSpecificIndex).sqrMagnitude - Value) < 0.0025f;

        public override IBlackboardValue GetNewObject(uint typeSpecificIndex, Vector3 value) => 
            new VectorHasTheSameMagnitudeAsValue(typeSpecificIndex, value);
    }

    public class VectorDoesNotHaveTheSameMagnitudeAsValue : TemplateValue<Vector3>
    {
        public VectorDoesNotHaveTheSameMagnitudeAsValue(uint typeSpecificIndex, Vector3 value) : base(typeSpecificIndex, value) => 
            Value = value.sqrMagnitude;

        private new float Value { get; }

        public override bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) => 
            Mathf.Abs(dataSet.GetVector(TypeSpecificIndex).sqrMagnitude - Value) > 0.0025f;

        public override IBlackboardValue GetNewObject(uint typeSpecificIndex, Vector3 value) => 
            new VectorDoesNotHaveTheSameMagnitudeAsValue(typeSpecificIndex, value);
    }
}