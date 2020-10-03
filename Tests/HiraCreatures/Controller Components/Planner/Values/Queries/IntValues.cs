using HiraEngine.Components.Planner.Internal;
using NUnit.Framework;
using UnityEngine;
using static UnityEngine.BlackboardTypes;
using Assert = UnityEngine.Assertions.Assert;

namespace HiraTests.HiraEngine.Components.Planner
{
    public class IntValues
    {
        private static IReadWriteBlackboardDataSet GetDataSetWithValue(uint index, int value)
        {
            var dataSet = GetWriteableDataSet(intKeyCount: index + 1);
            dataSet.Integers[index] = value;
            return dataSet;
        }

        private static IReadWriteBlackboardDataSet[] GetDataSetsWithValue(uint index, int value)
        {
            return new[]
            {
                GetDataSetWithValue(index, value - 5),
                GetDataSetWithValue(index, value),
                GetDataSetWithValue(index, value + 5)
            };
        }

        private static void PerformTests(IBlackboardQuery bValue, uint index, int value, bool zero, bool one, bool two)
        {
            var dataSets = GetDataSetsWithValue(index, value);

            Assert.AreEqual(zero, bValue.IsSatisfiedBy(dataSets[0]));
            Assert.AreEqual(one, bValue.IsSatisfiedBy(dataSets[1]));
            Assert.AreEqual(two, bValue.IsSatisfiedBy(dataSets[2]));
        }

        [Test]
        public void int_equals_value([Random(0u, 10u, 3)] uint index,
            [Random(int.MinValue, int.MaxValue, 3)]
            int value)
        {
            var intEqualsValue = new IntEqualsValue(index, value);
            PerformTests(intEqualsValue, index, value, false, true, false);
        }

        [Test]
        public void int_does_not_equal_value([Random(0u, 10u, 3)] uint index,
            [Random(int.MinValue, int.MaxValue, 3)]
            int value)
        {
            var intDoesNotEqualValue = new IntDoesNotEqualValue(index, value);
            PerformTests(intDoesNotEqualValue, index, value, true, false, true);
        }

        [Test]
        public void int_greater_than_value([Random(0u, 10u, 3)] uint index,
            [Random(int.MinValue, int.MaxValue, 3)]
            int value)
        {
            var intGreaterThanValue = new IntGreaterThanValue(index, value);
            PerformTests(intGreaterThanValue, index, value, false, false, true);
        }

        [Test]
        public void int_greater_than_or_equals_value([Random(0u, 10u, 3)] uint index,
            [Random(int.MinValue, int.MaxValue, 3)]
            int value)
        {
            var intGreaterThanOrEqualToValue = new IntGreaterThanOrEqualToValue(index, value);
            PerformTests(intGreaterThanOrEqualToValue, index, value, false, true, true);
        }

        [Test]
        public void int_lesser_than_value([Random(0u, 10u, 3)] uint index,
            [Random(int.MinValue, int.MaxValue, 3)]
            int value)
        {
            var intLesserThanValue = new IntLesserThanValue(index, value);
            PerformTests(intLesserThanValue, index, value, true, false, false);
        }

        [Test]
        public void int_lesser_than_or_equals_value([Random(0u, 10u, 3)] uint index,
            [Random(int.MinValue, int.MaxValue, 3)]
            int value)
        {
            var intLesserThanOrEqualToValue = new IntLesserThanOrEqualToValue(index, value);
            PerformTests(intLesserThanOrEqualToValue, index, value, true, true, false);
        }
    }
}