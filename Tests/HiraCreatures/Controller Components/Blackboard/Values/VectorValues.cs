using HiraEngine.Components.Blackboard;
using HiraEngine.Components.Blackboard.Internal;
using NUnit.Framework;
using UnityEngine;
using static UnityEngine.BlackboardTypes;
using Assert = UnityEngine.Assertions.Assert;

namespace HiraTests.HiraEngine.Components.Blackboard
{
    public class VectorValues
    {
        private static IReadWriteBlackboardDataSet GetDataSetWithValue(uint index, Vector3 value)
        {
            var dataSet = GetWriteableDataSet(vectorKeyCount: index + 1);
            dataSet.Vectors[index] = value;
            return dataSet;
        }

        private static IReadWriteBlackboardDataSet[] GetDataSetsWithValue(uint index, Vector3 value)
        {
            return new[]
            {
                GetDataSetWithValue(index, value),
                GetDataSetWithValue(index, value * 5),
                GetDataSetWithValue(index, value * 0.25f),
                GetDataSetWithValue(index, value.normalized),
                GetDataSetWithValue(index, value * -1),
                GetDataSetWithValue(index, Vector3.Cross(value, Vector3.one))
            };
        }

        private static void PerformTests(IBlackboardValue bValue, uint index, Vector3 value,
            bool? same = default,
            bool? extended = default,
            bool? shrunk = default,
            bool? normalized = default,
            bool? inverted = default,
            bool? perpendicular = default)
        {
            var dataSets = GetDataSetsWithValue(index, value);

            if (same != null) Assert.AreEqual(same, bValue.IsSatisfiedBy(dataSets[0]));
            if (extended != null) Assert.AreEqual(extended, bValue.IsSatisfiedBy(dataSets[1]));
            if (shrunk != null) Assert.AreEqual(shrunk, bValue.IsSatisfiedBy(dataSets[2]));
            if (normalized != null) Assert.AreEqual(normalized, bValue.IsSatisfiedBy(dataSets[3]));
            if (inverted != null) Assert.AreEqual(inverted, bValue.IsSatisfiedBy(dataSets[4]));
            if (perpendicular != null) Assert.AreEqual(perpendicular, bValue.IsSatisfiedBy(dataSets[5]));
        }

        [Test]
        public void vector_equals_value([Random(0u, 10u, 1)] uint index,
            [Random(-1000f, 1000f, 2)] float x,
            [Random(-1000f, 1000f, 2)] float y,
            [Random(-1000f, 1000f, 2)] float z)
        {
            var value = new Vector3(x, y, z);

            var vectorEqualsValue = new VectorEqualsValue(index, value);

            PerformTests(vectorEqualsValue, index, value, true, false, false, false, false, false);
        }

        [Test]
        public void vector_does_not_equal_value([Random(0u, 10u, 1)] uint index,
            [Random(-1000f, 1000f, 2)] float x,
            [Random(-1000f, 1000f, 2)] float y,
            [Random(-1000f, 1000f, 2)] float z)
        {
            var value = new Vector3(x, y, z);

            var vectorDoesNotEqualValue = new VectorDoesNotEqualValue(index, value);

            PerformTests(vectorDoesNotEqualValue, index, value, false, true, true, true, true, true);
        }

        [Test]
        public void vector_is_parallel_to_value([Random(0u, 10u, 1)] uint index,
            [Random(-1000f, 1000f, 2)] float x,
            [Random(-1000f, 1000f, 2)] float y,
            [Random(-1000f, 1000f, 2)] float z)
        {
            var value = new Vector3(x, y, z);

            var vectorIsParallelToValue = new VectorIsParallelToValue(index, value);

            PerformTests(vectorIsParallelToValue, index, value, true, true, true, true, false, false);
        }

        [Test]
        public void vector_is_not_parallel_to_value([Random(0u, 10u, 1)] uint index,
            [Random(-1000f, 1000f, 2)] float x,
            [Random(-1000f, 1000f, 2)] float y,
            [Random(-1000f, 1000f, 2)] float z)
        {
            var value = new Vector3(x, y, z);

            var vectorIsNotParallelToValue = new VectorIsNotParallelToValue(index, value);

            PerformTests(vectorIsNotParallelToValue, index, value, false, false, false, false, true, true);
        }

        [Test]
        public void vector_is_perpendicular_to_value([Random(0u, 10u, 1)] uint index,
            [Random(-1000f, 1000f, 2)] float x,
            [Random(-1000f, 1000f, 2)] float y,
            [Random(-1000f, 1000f, 2)] float z)
        {
            var value = new Vector3(x, y, z);

            var vectorIsPerpendicularToValue = new VectorIsPerpendicularToValue(index, value);

            PerformTests(vectorIsPerpendicularToValue, index, value, false, false, false, false, false, true);
        }

        [Test]
        public void vector_is_not_perpendicular_to_value([Random(0u, 10u, 1)] uint index,
            [Random(-1000f, 1000f, 2)] float x,
            [Random(-1000f, 1000f, 2)] float y,
            [Random(-1000f, 1000f, 2)] float z)
        {
            var value = new Vector3(x, y, z);

            var vectorIsNotPerpendicularToValue = new VectorIsNotPerpendicularToValue(index, value);

            PerformTests(vectorIsNotPerpendicularToValue, index, value, true, true, true, true, true, false);
        }

        [Test]
        public void vector_is_anti_parallel_to_value([Random(0u, 10u, 1)] uint index,
            [Random(-1000f, 1000f, 2)] float x,
            [Random(-1000f, 1000f, 2)] float y,
            [Random(-1000f, 1000f, 2)] float z)
        {
            var value = new Vector3(x, y, z);

            var vectorIsAntiParallelToValue = new VectorIsAntiParallelToValue(index, value);

            PerformTests(vectorIsAntiParallelToValue, index, value, false, false, false, false, true, false);
        }

        [Test]
        public void vector_is_not_anti_parallel_to_value([Random(0u, 10u, 1)] uint index,
            [Random(-1000f, 1000f, 2)] float x,
            [Random(-1000f, 1000f, 2)] float y,
            [Random(-1000f, 1000f, 2)] float z)
        {
            var value = new Vector3(x, y, z);

            var vectorIsNotAntiParallelToValue = new VectorIsNotAntiParallelToValue(index, value);

            PerformTests(vectorIsNotAntiParallelToValue, index, value, true, true, true, true, false, true);
        }

        [Test]
        public void vector_has_magnitude_greater_than_value([Random(0u, 10u, 1)] uint index,
            [Random(-1000f, 1000f, 2)] float x,
            [Random(-1000f, 1000f, 2)] float y,
            [Random(-1000f, 1000f, 2)] float z)
        {
            var value = new Vector3(x, y, z);

            var vectorHasAHigherMagnitudeThanValue = new VectorHasAHigherMagnitudeThanValue(index, value);

            PerformTests(vectorHasAHigherMagnitudeThanValue, index, value, false, true, false, null, false, null);
        }

        [Test]
        public void vector_has_magnitude_lesser_than_value([Random(0u, 10u, 1)] uint index,
            [Random(-1000f, 1000f, 2)] float x,
            [Random(-1000f, 1000f, 2)] float y,
            [Random(-1000f, 1000f, 2)] float z)
        {
            var value = new Vector3(x, y, z);

            var vectorHasALowerMagnitudeThanValue = new VectorHasALowerMagnitudeThanValue(index, value);

            PerformTests(vectorHasALowerMagnitudeThanValue, index, value, false, false, true, null, false, null);
        }

        [Test]
        public void vector_has_magnitude_equal_to_value([Random(0u, 10u, 1)] uint index,
            [Random(-1000f, 1000f, 2)] float x,
            [Random(-1000f, 1000f, 2)] float y,
            [Random(-1000f, 1000f, 2)] float z)
        {
            var value = new Vector3(x, y, z);

            var vectorHasTheSameMagnitudeAsValue = new VectorHasTheSameMagnitudeAsValue(index, value);

            PerformTests(vectorHasTheSameMagnitudeAsValue, index, value, true, false, false, null, true, null);
        }

        [Test]
        public void vector_has_magnitude_not_equal_to_value([Random(0u, 10u, 1)] uint index,
            [Random(-1000f, 1000f, 2)] float x,
            [Random(-1000f, 1000f, 2)] float y,
            [Random(-1000f, 1000f, 2)] float z)
        {
            var value = new Vector3(x, y, z);

            var vectorDoesNotHaveTheSameMagnitudeAsValue = new VectorDoesNotHaveTheSameMagnitudeAsValue(index, value);

            PerformTests(vectorDoesNotHaveTheSameMagnitudeAsValue, index, value, false, true, true, null, false, null);
        }
    }
}