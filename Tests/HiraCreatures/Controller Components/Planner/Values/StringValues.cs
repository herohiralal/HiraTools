using HiraEngine.Components.Planner.Internal;
using NUnit.Framework;
using UnityEngine;
using static UnityEngine.BlackboardTypes;
using Assert = UnityEngine.Assertions.Assert;

namespace HiraTests.HiraEngine.Components.Planner
{
    public class StringValues
    {
        private const string garbage = "saf90xzjcc-=4j=";

        private static IReadWriteBlackboardDataSet GetDataSetWithValue(uint index, string value)
        {
            var dataSet = GetWriteableDataSet(stringKeyCount: index + 1);
            dataSet.Strings[index] = value;
            return dataSet;
        }

        private static IReadWriteBlackboardDataSet[] GetDataSetsWithValue(uint index, string value)
        {
            return new[]
            {
                GetDataSetWithValue(index, value),
                GetDataSetWithValue(index, garbage),
                GetDataSetWithValue(index, garbage + value + garbage),
                GetDataSetWithValue(index, "x"),
                GetDataSetWithValue(index, garbage + value),
                GetDataSetWithValue(index, value + garbage),
                GetDataSetWithValue(index, value.Replace("ing", "")),
                GetDataSetWithValue(index, value.Replace("Som", "")),
                GetDataSetWithValue(index, value.Replace("Som", "").Replace("ing", ""))
            };
        }

        private static void PerformTests(IBlackboardQuery bValue, uint index, string value,
            bool same = default,
            bool random = default,
            bool padded = default,
            bool x = default,
            bool valueAtEnd = default,
            bool valueAtStart = default,
            bool trimmedAtEnd = default,
            bool trimmedAtStart = default,
            bool trimmedOnBothEnds = default)
        {
            var dataSets = GetDataSetsWithValue(index, value);

            Assert.AreEqual(same, bValue.IsSatisfiedBy(dataSets[0]));
            Assert.AreEqual(random, bValue.IsSatisfiedBy(dataSets[1]));
            Assert.AreEqual(padded, bValue.IsSatisfiedBy(dataSets[2]));
            Assert.AreEqual(x, bValue.IsSatisfiedBy(dataSets[3]));
            Assert.AreEqual(valueAtEnd, bValue.IsSatisfiedBy(dataSets[4]));
            Assert.AreEqual(valueAtStart, bValue.IsSatisfiedBy(dataSets[5]));
            Assert.AreEqual(trimmedAtEnd, bValue.IsSatisfiedBy(dataSets[6]));
            Assert.AreEqual(trimmedAtStart, bValue.IsSatisfiedBy(dataSets[7]));
            Assert.AreEqual(trimmedOnBothEnds, bValue.IsSatisfiedBy(dataSets[8]));
        }

        [Test]
        public void string_equals_value([Random(0u, 10u, 1)] uint index, [Values("Something", "Some other thing")]
            string value)
        {
            var stringEqualsValue = new StringEqualsValue(index, value);

            PerformTests(stringEqualsValue, index, value, same: true);
        }

        [Test]
        public void string_does_not_equal_value([Random(0u, 10u, 1)] uint index,
            [Values("Something", "Some other thing")]
            string value)
        {
            var stringDoesNotEqualValue = new StringDoesNotEqualValue(index, value);

            PerformTests(stringDoesNotEqualValue, index, value, false, true, true, true, true, true, true, true, true);
        }

        [Test]
        public void string_contains_value([Random(0u, 10u, 1)] uint index, [Values("Something", "Some other thing")]
            string value)
        {
            var stringContainsValue = new StringContainsValue(index, value);

            PerformTests(stringContainsValue, index, value, true, false, true, false, true, true);
        }

        [Test]
        public void string_contained_by_value([Random(0u, 10u, 1)] uint index, [Values("Something", "Some other thing")]
            string value)
        {
            var stringContainedByValue = new StringContainedByValue(index, value);

            PerformTests(stringContainedByValue, index, value, same: true, trimmedAtEnd: true, trimmedAtStart: true,
                trimmedOnBothEnds: true);
        }

        [Test]
        public void string_does_not_contain_value([Random(0u, 10u, 1)] uint index,
            [Values("Something", "Some other thing")]
            string value)
        {
            var stringDoesNotContainValue = new StringDoesNotContainValue(index, value);

            PerformTests(stringDoesNotContainValue, index, value, random: true, x: true, trimmedAtEnd: true,
                trimmedAtStart: true, trimmedOnBothEnds: true);
        }

        [Test]
        public void string_not_contained_by_value([Random(0u, 10u, 1)] uint index,
            [Values("Something", "Some other thing")]
            string value)
        {
            var stringNotContainedByValue = new StringNotContainedByValue(index, value);

            PerformTests(stringNotContainedByValue, index, value, random: true, padded: true, x: true, valueAtEnd: true, valueAtStart: true);
        }

        [Test]
        public void string_starts_with_value([Random(0u, 10u, 1)] uint index, [Values("Something", "Some other thing")]
            string value)
        {
            var stringStartsWithValue = new StringStartsWithValue(index, value);

            PerformTests(stringStartsWithValue, index, value, same: true, valueAtStart: true);
        }

        [Test]
        public void string_does_not_start_with_value([Random(0u, 10u, 1)] uint index,
            [Values("Something", "Some other thing")]
            string value)
        {
            var stringDoesNotStartWithValue = new StringDoesNotStartWithValue(index, value);

            PerformTests(stringDoesNotStartWithValue, index, value, random: true, padded: true, x: true,
                valueAtEnd: true, trimmedAtStart: true, trimmedAtEnd: true, trimmedOnBothEnds: true);
        }

        [Test]
        public void string_ends_with_value([Random(0u, 10u, 1)] uint index, [Values("Something", "Some other thing")]
            string value)
        {
            var stringEndsWithValue = new StringEndsWithValue(index, value);

            PerformTests(stringEndsWithValue, index, value, same: true, valueAtEnd: true);
        }

        [Test]
        public void string_does_not_end_with_value([Random(0u, 10u, 1)] uint index,
            [Values("Something", "Some other thing")]
            string value)
        {
            var stringDoesNotEndWithValue = new StringDoesNotEndWithValue(index, value);

            PerformTests(stringDoesNotEndWithValue, index, value, random: true, padded: true, x: true,
                valueAtStart: true, trimmedAtStart: true, trimmedAtEnd: true, trimmedOnBothEnds: true);
        }
    }
}