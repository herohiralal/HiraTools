using UnityEngine;

namespace HiraCreatures.Components.Blackboard.Internal.Values
{
    public class StringEqualsValue : TemplateValue<string>
    {
        public StringEqualsValue(uint typeSpecificIndex, string value) : base(typeSpecificIndex, value)
        {
        }

        public override bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) =>
            dataSet.GetString(TypeSpecificIndex) == Value;

        public override IBlackboardValue GetNewObject(uint typeSpecificIndex, string value) =>
            new StringEqualsValue(typeSpecificIndex, value);
    }

    public class StringDoesNotEqualValue : TemplateValue<string>
    {
        public StringDoesNotEqualValue(uint typeSpecificIndex, string value) : base(typeSpecificIndex, value)
        {
        }

        public override bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) => 
            dataSet.GetString(TypeSpecificIndex) != Value;

        public override IBlackboardValue GetNewObject(uint typeSpecificIndex, string value) => 
            new StringDoesNotEqualValue(typeSpecificIndex, value);
    }

    public class StringContainsValue : TemplateValue<string>
    {
        public StringContainsValue(uint typeSpecificIndex, string value) : base(typeSpecificIndex, value)
        {
        }

        public override bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) =>
            dataSet.GetString(TypeSpecificIndex).Contains(Value);

        public override IBlackboardValue GetNewObject(uint typeSpecificIndex, string value) =>
            new StringContainsValue(typeSpecificIndex, value);
    }

    public class StringIncludedByValue : TemplateValue<string>
    {
        public StringIncludedByValue(uint typeSpecificIndex, string value) : base(typeSpecificIndex, value)
        {
        }

        public override bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) =>
            Value.Contains(dataSet.GetString(TypeSpecificIndex));

        public override IBlackboardValue GetNewObject(uint typeSpecificIndex, string value) =>
            new StringIncludedByValue(typeSpecificIndex, value);
    }

    public class StringDoesNotContainValue : TemplateValue<string>
    {
        public StringDoesNotContainValue(uint typeSpecificIndex, string value) : base(typeSpecificIndex, value)
        {
        }

        public override bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) =>
            !dataSet.GetString(TypeSpecificIndex).Contains(Value);

        public override IBlackboardValue GetNewObject(uint typeSpecificIndex, string value) =>
            new StringDoesNotContainValue(typeSpecificIndex, value);
    }

    public class StringNotContainedByValue : TemplateValue<string>
    {
        public StringNotContainedByValue(uint typeSpecificIndex, string value) : base(typeSpecificIndex, value)
        {
        }

        public override bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) =>
            !Value.Contains(dataSet.GetString(TypeSpecificIndex));

        public override IBlackboardValue GetNewObject(uint typeSpecificIndex, string value) =>
            new StringNotContainedByValue(typeSpecificIndex, value);
    }

    public class StringStartsWithValue : TemplateValue<string>
    {
        public StringStartsWithValue(uint typeSpecificIndex, string value) : base(typeSpecificIndex, value)
        {
        }

        public override bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) =>
            dataSet.GetString(TypeSpecificIndex).StartsWith(Value);

        public override IBlackboardValue GetNewObject(uint typeSpecificIndex, string value) =>
            new StringStartsWithValue(typeSpecificIndex, value);
    }

    public class StringDoesNotStartWith : TemplateValue<string>
    {
        public StringDoesNotStartWith(uint typeSpecificIndex, string value) : base(typeSpecificIndex, value)
        {
        }

        public override bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) =>
            !dataSet.GetString(TypeSpecificIndex).StartsWith(Value);

        public override IBlackboardValue GetNewObject(uint typeSpecificIndex, string value) =>
            new StringDoesNotStartWith(typeSpecificIndex, value);
    }

    public class StringEndsWith : TemplateValue<string>
    {
        public StringEndsWith(uint typeSpecificIndex, string value) : base(typeSpecificIndex, value)
        {
        }

        public override bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) =>
            dataSet.GetString(TypeSpecificIndex).EndsWith(Value);

        public override IBlackboardValue GetNewObject(uint typeSpecificIndex, string value) =>
            new StringEndsWith(typeSpecificIndex, value);
    }

    public class StringDoesNotEndWith : TemplateValue<string>
    {
        public StringDoesNotEndWith(uint typeSpecificIndex, string value) : base(typeSpecificIndex, value)
        {
        }

        public override bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) =>
            !dataSet.GetString(TypeSpecificIndex).EndsWith(Value);

        public override IBlackboardValue GetNewObject(uint typeSpecificIndex, string value) =>
            new StringDoesNotEndWith(typeSpecificIndex, value);
    }
}