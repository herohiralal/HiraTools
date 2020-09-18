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
}