using UnityEngine;

namespace HiraCreatures.Components.Blackboard.Internal.Values
{
    public class IntEqualsValue : TemplateValue<int>
    {
        public IntEqualsValue(uint typeSpecificIndex, int value) : base(typeSpecificIndex, value)
        {
        }

        public override bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) => 
            dataSet.GetInteger(TypeSpecificIndex) == Value;

        public override IBlackboardValue GetNewObject(uint typeSpecificIndex, int value) => 
            new IntEqualsValue(typeSpecificIndex, value);
    }

    public class IntDoesNotEqualValue : TemplateValue<int>
    {
        public IntDoesNotEqualValue(uint typeSpecificIndex, int value) : base(typeSpecificIndex, value)
        {
        }

        public override bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) => 
            dataSet.GetInteger(TypeSpecificIndex) != Value;

        public override IBlackboardValue GetNewObject(uint typeSpecificIndex, int value) => 
            new IntDoesNotEqualValue(typeSpecificIndex, value);
    }
    
    public class IntGreaterThanValue : TemplateValue<int>
    {
        public IntGreaterThanValue(uint typeSpecificIndex, int value) : base(typeSpecificIndex, value)
        {
        }

        public override bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) => 
            dataSet.GetInteger(TypeSpecificIndex) > Value;

        public override IBlackboardValue GetNewObject(uint typeSpecificIndex, int value) => 
            new IntGreaterThanValue(typeSpecificIndex, value);
    }

    public class IntGreaterThanOrEqualToValue : TemplateValue<int>
    {
        public IntGreaterThanOrEqualToValue(uint typeSpecificIndex, int value) : base(typeSpecificIndex, value)
        {
        }

        public override bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) => 
            dataSet.GetInteger(TypeSpecificIndex) >= Value;

        public override IBlackboardValue GetNewObject(uint typeSpecificIndex, int value) => 
            new IntGreaterThanOrEqualToValue(typeSpecificIndex, value);
    }

    public class IntLesserThanValue : TemplateValue<int>
    {
        public IntLesserThanValue(uint typeSpecificIndex, int value) : base(typeSpecificIndex, value)
        {
        }

        public override bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) => 
            dataSet.GetInteger(TypeSpecificIndex) < Value;

        public override IBlackboardValue GetNewObject(uint typeSpecificIndex, int value) => 
            new IntLesserThanValue(typeSpecificIndex, value);
    }

    public class IntLesserThanOrEqualToValue : TemplateValue<int>
    {
        public IntLesserThanOrEqualToValue(uint typeSpecificIndex, int value) : base(typeSpecificIndex, value)
        {
        }

        public override bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) => 
            dataSet.GetInteger(TypeSpecificIndex) <= Value;

        public override IBlackboardValue GetNewObject(uint typeSpecificIndex, int value) => 
            new IntLesserThanOrEqualToValue(typeSpecificIndex, value);
    }
}