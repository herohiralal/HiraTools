using HiraCreatures.Components.Blackboard.Helpers;
using NUnit.Framework;

namespace Hiralal.Tests.Components.Blackboard
{
    [TestFixture]
    public class DataSet
    {
        [Test]
        public void duplicating_a_data_set_should_give_a_new_object(
            [Random(0u, 20u, 2)] uint boolCount,
            [Random(0u, 20u, 2)] uint floatCount,
            [Random(0u, 20u, 2)] uint intCount,
            [Random(0u, 20u, 2)] uint stringCount,
            [Random(0u, 20u, 2)] uint vectorCount
        )
        {
            var dataSet =
                BlackboardTypes.GetWriteableDataSet(boolCount, floatCount, intCount, stringCount, vectorCount);
            var duplicate = dataSet.GetDuplicate();

            Assert.AreNotSame(duplicate, dataSet);
            Assert.AreNotSame(duplicate.Booleans, dataSet.Booleans);
            Assert.AreNotSame(duplicate.Floats, dataSet.Floats);
            Assert.AreNotSame(duplicate.Integers, dataSet.Integers);
            Assert.AreNotSame(duplicate.Strings, dataSet.Strings);
            Assert.AreNotSame(duplicate.Vectors, dataSet.Vectors);
            Assert.AreEqual(boolCount, duplicate.Booleans.Length);
            Assert.AreEqual(floatCount, duplicate.Floats.Length);
            Assert.AreEqual(intCount, duplicate.Integers.Length);
            Assert.AreEqual(stringCount, duplicate.Strings.Length);
            Assert.AreEqual(vectorCount, duplicate.Vectors.Length);
        }

        [Test]
        public void duplicating_a_data_set_should_duplicate_values(
            [Random(float.MinValue, float.MaxValue, 2)]
            float first,
            [Random(float.MinValue, float.MaxValue, 2)]
            float second,
            [Random(float.MinValue, float.MaxValue, 2)]
            float third)
        {
            var dataSet = BlackboardTypes.GetWriteableDataSet(floatKeyCount: 3);
            (dataSet.Floats[0], dataSet.Floats[1], dataSet.Floats[2]) = (first, second, third);

            var duplicate = dataSet.GetDuplicate();

            Assert.AreEqual(first, duplicate.Floats[0]);
            Assert.AreEqual(second, duplicate.Floats[1]);
            Assert.AreEqual(third, duplicate.Floats[2]);

            (dataSet.Floats[0], dataSet.Floats[1], dataSet.Floats[2]) = (third, first, second);

            Assert.AreEqual(first, duplicate.Floats[0]);
            Assert.AreEqual(second, duplicate.Floats[1]);
            Assert.AreEqual(third, duplicate.Floats[2]);
        }
    }
}