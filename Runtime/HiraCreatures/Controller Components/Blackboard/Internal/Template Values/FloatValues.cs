using UnityEngine;

namespace HiraEngine.Components.Blackboard.Internal
{
    public class FloatGreaterThanValue : TemplateValue<float>
    {
        public FloatGreaterThanValue(uint typeSpecificIndex, float value) : base(typeSpecificIndex, value)
        {
        }

        public override bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) => 
            dataSet.GetFloat(TypeSpecificIndex) > Value;

        public override IBlackboardValue GetNewObject(uint typeSpecificIndex, float value) => 
            new FloatGreaterThanValue(typeSpecificIndex, value);
    }

    public class FloatGreaterThanOrEqualToValue : TemplateValue<float>
    {
        public FloatGreaterThanOrEqualToValue(uint typeSpecificIndex, float value) : base(typeSpecificIndex, value)
        {
        }

        public override bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) => 
            dataSet.GetFloat(TypeSpecificIndex) >= Value;

        public override IBlackboardValue GetNewObject(uint typeSpecificIndex, float value) => 
            new FloatGreaterThanOrEqualToValue(typeSpecificIndex, value);
    }

    public class FloatLesserThanValue : TemplateValue<float>
    {
        public FloatLesserThanValue(uint typeSpecificIndex, float value) : base(typeSpecificIndex, value)
        {
        }

        public override bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) => 
            dataSet.GetFloat(TypeSpecificIndex) < Value;

        public override IBlackboardValue GetNewObject(uint typeSpecificIndex, float value) => 
            new FloatLesserThanValue(typeSpecificIndex, value);
    }

    public class FloatLesserThanOrEqualToValue : TemplateValue<float>
    {
        public FloatLesserThanOrEqualToValue(uint typeSpecificIndex, float value) : base(typeSpecificIndex, value)
        {
        }

        public override bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) => 
            dataSet.GetFloat(TypeSpecificIndex) <= Value;

        public override IBlackboardValue GetNewObject(uint typeSpecificIndex, float value) => 
            new FloatLesserThanOrEqualToValue(typeSpecificIndex, value);
    }
}