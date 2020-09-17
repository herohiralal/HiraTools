﻿using UnityEngine;

namespace HiraCreatures.Components.Blackboard.Internal.Values
{
    public class StringEqualsValue : TemplateValue<string>
    {
        public StringEqualsValue(uint typeSpecificIndex, string value) : base(typeSpecificIndex, value)
        {
        }

        public override bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) => 
            dataSet.GetString(TypeSpecificIndex) == Value;
    }

    public class StringContainsValue : TemplateValue<string>
    {
        public StringContainsValue(uint typeSpecificIndex, string value) : base(typeSpecificIndex, value)
        {
        }

        public override bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) => 
            dataSet.GetString(TypeSpecificIndex).Contains(Value);
    }

    public class StringIncludedByValue : TemplateValue<string>
    {
        public StringIncludedByValue(uint typeSpecificIndex, string value) : base(typeSpecificIndex, value)
        {
        }

        public override bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) => 
            Value.Contains(dataSet.GetString(TypeSpecificIndex));
    }

    public class StringDoesNotContainValue : TemplateValue<string>
    {
        public StringDoesNotContainValue(uint typeSpecificIndex, string value) : base(typeSpecificIndex, value)
        {
        }

        public override bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) => 
            !dataSet.GetString(TypeSpecificIndex).Contains(Value);
    }

    public class StringNotContainedByValue : TemplateValue<string>
    {
        public StringNotContainedByValue(uint typeSpecificIndex, string value) : base(typeSpecificIndex, value)
        {
        }

        public override bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) => 
            !Value.Contains(dataSet.GetString(TypeSpecificIndex));
    }

    public class StringStartsWithValue : TemplateValue<string>
    {
        public StringStartsWithValue(uint typeSpecificIndex, string value) : base(typeSpecificIndex, value)
        {
        }

        public override bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) => 
            dataSet.GetString(TypeSpecificIndex).StartsWith(Value);
    }

    public class StringDoesNotStartWith : TemplateValue<string>
    {
        public StringDoesNotStartWith(uint typeSpecificIndex, string value) : base(typeSpecificIndex, value)
        {
        }

        public override bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) => 
            !dataSet.GetString(TypeSpecificIndex).StartsWith(Value);
    }

    public class StringEndsWith : TemplateValue<string>
    {
        public StringEndsWith(uint typeSpecificIndex, string value) : base(typeSpecificIndex, value)
        {
        }

        public override bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) => 
            dataSet.GetString(TypeSpecificIndex).EndsWith(Value);
    }

    public class StringDoesNotEndWith : TemplateValue<string>
    {
        public StringDoesNotEndWith(uint typeSpecificIndex, string value) : base(typeSpecificIndex, value)
        {
        }

        public override bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet) => 
            !dataSet.GetString(TypeSpecificIndex).EndsWith(Value);
    }
}