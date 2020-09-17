using UnityEngine;

namespace HiraCreatures.Components.Blackboard.Internal.Values
{
    public class FloatGreaterThanValue : TemplateValue<float>
    {
        public FloatGreaterThanValue(uint typeSpecificIndex, float value) : base(typeSpecificIndex, value)
        {
        }

        public override bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) => 
            dataSet.GetFloat(TypeSpecificIndex) > Value;
    }

    public class FloatGreaterThanOrEqualToValue : TemplateValue<float>
    {
        public FloatGreaterThanOrEqualToValue(uint typeSpecificIndex, float value) : base(typeSpecificIndex, value)
        {
        }

        public override bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) => 
            dataSet.GetFloat(TypeSpecificIndex) >= Value;
    }

    public class FloatLesserThanValue : TemplateValue<float>
    {
        public FloatLesserThanValue(uint typeSpecificIndex, float value) : base(typeSpecificIndex, value)
        {
        }

        public override bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) => 
            dataSet.GetFloat(TypeSpecificIndex) < Value;
    }

    public class FloatLesserThanOrEqualToValue : TemplateValue<float>
    {
        public FloatLesserThanOrEqualToValue(uint typeSpecificIndex, float value) : base(typeSpecificIndex, value)
        {
        }

        public override bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) => 
            dataSet.GetFloat(TypeSpecificIndex) <= Value;
    }
}