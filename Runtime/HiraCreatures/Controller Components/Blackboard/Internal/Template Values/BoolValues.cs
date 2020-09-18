using UnityEngine;

namespace HiraCreatures.Components.Blackboard.Internal.Values
{
    public class BoolEqualsValue : TemplateValue<bool>
    {
        public BoolEqualsValue(uint typeSpecificIndex, bool value) : base(typeSpecificIndex, value)
        {
        }

        public override bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) => 
            dataSet.GetBoolean(TypeSpecificIndex) == Value;

        public override IBlackboardValue GetNewObject(uint typeSpecificIndex, bool value) => 
            new BoolEqualsValue(typeSpecificIndex, value);
    }
}