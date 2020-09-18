using HiraCreatures.Components.Blackboard;
using HiraCreatures.Components.Blackboard.Internal.Values;
using NUnit.Framework;
using UnityEngine;
using static HiraCreatures.Components.Blackboard.Helpers.BlackboardTypes;
using Assert = UnityEngine.Assertions.Assert;

namespace Hiralal.Components.Blackboard.Tests.ValueTests
{
    public class FloatValueTests
    {
        private static IReadWriteBlackboardDataSet GetDataSetWithValue(uint index, float value)
        {
            var dataSet = GetWriteableDataSet(floatKeyCount: index + 1);
            dataSet.Floats[index] = value;
            return dataSet;
        }

        private static IReadWriteBlackboardDataSet[] GetDataSetsWithValue(uint index, float value)
        {
            return new[]
            {
                GetDataSetWithValue(index, value - 5f),
                GetDataSetWithValue(index, value),
                GetDataSetWithValue(index, value + 5f)
            };
        }

        private static void PerformTests(IBlackboardValue bValue, uint index, float value, bool zero, bool one, bool two)
        {
            var dataSets = GetDataSetsWithValue(index, value);

            Assert.AreEqual(zero, bValue.IsSatisfiedBy(dataSets[0]));
            Assert.AreEqual(one, bValue.IsSatisfiedBy(dataSets[1]));
            Assert.AreEqual(two, bValue.IsSatisfiedBy(dataSets[2]));
        }

        [Test]
        public void float_greater_than_value([Random(0u, 10u, 3)] uint index,
            [Random(-1000f, 1000f, 3)]
            float value)
        {
            var floatGreaterThanValue = new FloatGreaterThanValue(index, value);
            PerformTests(floatGreaterThanValue, index, value, false, false, true);
        }

        [Test]
        public void float_greater_than_or_equals_value([Random(0u, 10u, 3)] uint index,
            [Random(-1000f, 1000f, 3)]
            float value)
        {
            var floatGreaterThanOrEqualToValue = new FloatGreaterThanOrEqualToValue(index, value);
            PerformTests(floatGreaterThanOrEqualToValue, index, value, false, true, true);
        }

        [Test]
        public void float_lesser_than_value([Random(0u, 10u, 3)] uint index,
            [Random(-1000f, 1000f, 3)]
            float value)
        {
            var floatLesserThanValue = new FloatLesserThanValue(index, value);
            PerformTests(floatLesserThanValue, index, value, true, false, false);
        }

        [Test]
        public void float_lesser_than_or_equals_value([Random(0u, 10u, 3)] uint index,
            [Random(-1000f, 1000f, 3)]
            float value)
        {
            var floatLesserThanOrEqualToValue = new FloatLesserThanOrEqualToValue(index, value);
            PerformTests(floatLesserThanOrEqualToValue, index, value, true, true, false);
        }
    }
}