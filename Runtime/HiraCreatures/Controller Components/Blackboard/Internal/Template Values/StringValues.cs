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

    public class StringContainedByValue : TemplateValue<string>
    {
        public StringContainedByValue(uint typeSpecificIndex, string value) : base(typeSpecificIndex, value)
        {
        }

        public override bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) =>
            Value.Contains(dataSet.GetString(TypeSpecificIndex));

        public override IBlackboardValue GetNewObject(uint typeSpecificIndex, string value) =>
            new StringContainedByValue(typeSpecificIndex, value);
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

    public class StringDoesNotStartWithValue : TemplateValue<string>
    {
        public StringDoesNotStartWithValue(uint typeSpecificIndex, string value) : base(typeSpecificIndex, value)
        {
        }

        public override bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) =>
            !dataSet.GetString(TypeSpecificIndex).StartsWith(Value);

        public override IBlackboardValue GetNewObject(uint typeSpecificIndex, string value) =>
            new StringDoesNotStartWithValue(typeSpecificIndex, value);
    }

    public class StringEndsWithValue : TemplateValue<string>
    {
        public StringEndsWithValue(uint typeSpecificIndex, string value) : base(typeSpecificIndex, value)
        {
        }

        public override bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) =>
            dataSet.GetString(TypeSpecificIndex).EndsWith(Value);

        public override IBlackboardValue GetNewObject(uint typeSpecificIndex, string value) =>
            new StringEndsWithValue(typeSpecificIndex, value);
    }

    public class StringDoesNotEndWithValue : TemplateValue<string>
    {
        public StringDoesNotEndWithValue(uint typeSpecificIndex, string value) : base(typeSpecificIndex, value)
        {
        }

        public override bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) =>
            !dataSet.GetString(TypeSpecificIndex).EndsWith(Value);

        public override IBlackboardValue GetNewObject(uint typeSpecificIndex, string value) =>
            new StringDoesNotEndWithValue(typeSpecificIndex, value);
    }
}