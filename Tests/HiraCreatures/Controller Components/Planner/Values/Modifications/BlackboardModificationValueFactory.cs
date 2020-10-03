using System;
using HiraEngine.Components.Planner;
using HiraEngine.Components.Planner.Internal;
using NUnit.Framework;
using UnityEngine;
using static UnityEngine.BlackboardTypes;
using Assert = UnityEngine.Assertions.Assert;

namespace HiraTests.HiraEngine.Components.Planner
{
    [TestFixture]
    public class BlackboardModificationValueFactory
    {
        [Test]
        public void factory_creates_float_objects_properly_provided_a_correct_string()
        {
            var types = typeof(IBlackboardModificationDefaultObject<>).GetSubclasses();

            foreach (var type in types)
            {
                var reflectionName = type.GetReflectionName();
                var blackboardValue =
                    PlannerTypes.GetModification(reflectionName, new BlackboardValueConstructorParamsMock());
                Assert.AreEqual(type, blackboardValue.GetType());
            }
        }

        [Test]
        public void created_value_has_its_variables_in_accordance_with_params(
            [Random(0u, 10u, 3)] uint index,
            [Random(int.MinValue, int.MaxValue, 3)] int value,
            [Random(-5, 5, 1)] int noise)
        {
            var parameters = new BlackboardValueConstructorParamsMock(index, intValue: value);
            var typeString = typeof(IntEqualsValue).GetReflectionName();

            var dataSet = GetWriteableDataSet(intKeyCount: index + 1);
            dataSet.Integers[index] = value + noise;

            Assert.AreNotEqual(dataSet.Integers[index], value);

            var templateValue = PlannerTypes.GetModification(typeString, parameters);
            var undo = templateValue.ApplyTo(dataSet);

            Assert.AreEqual(dataSet.Integers[index], value);

            undo.ApplyTo(dataSet);

            Assert.AreNotEqual(dataSet.Integers[index], value);
        }
    }
}