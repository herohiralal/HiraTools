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
    public class BlackboardValueFactory
    {
        [Test]
        public void factory_creates_float_objects_properly_provided_a_correct_string()
        {
            var types = typeof(IBlackboardQueryDefaultObject<>).GetSubclasses();
            
            foreach (var type in types)
            {
                var reflectionName = type.GetReflectionName();
                var blackboardValue = PlannerTypes.GetQuery(reflectionName, new BlackboardValueConstructorParamsMock());
                Assert.AreEqual(type, blackboardValue.GetType());
            }
        }

        [Test]
        public void created_value_has_its_variables_in_accordance_with_params(
            [Random(0u, 10u, 3)] uint index, 
            [Random(int.MinValue, int.MaxValue, 3)] int value)
        {
            var parameters = new BlackboardValueConstructorParamsMock(index, intValue: value);
            var typeString = typeof(IntEqualsValue).GetReflectionName();

            var correctDataSet = GetWriteableDataSet(intKeyCount: index + 1);
            correctDataSet.Integers[index] = value;

            var incorrectDataSet = GetWriteableDataSet(intKeyCount: index + 1);
            incorrectDataSet.Integers[index] = value + 1;

            var templateValue = PlannerTypes.GetQuery(typeString, parameters);
            
            Assert.IsTrue(templateValue.IsSatisfiedBy(correctDataSet));
            Assert.IsFalse(templateValue.IsSatisfiedBy(incorrectDataSet));
        }
    }

    public class BlackboardValueConstructorParamsMock : IBlackboardValueConstructorParams
    {
        public BlackboardValueConstructorParamsMock(uint typeSpecificIndex = default,
            bool boolValue = default,
            float floatValue = default,
            int intValue = default,
            string stringValue = default,
            Vector3 vectorValue = default)
        {
            TypeSpecificIndex = typeSpecificIndex;
            BoolValue = boolValue;
            FloatValue = floatValue;
            IntValue = intValue;
            StringValue = stringValue;
            VectorValue = vectorValue;
        }

        public uint TypeSpecificIndex { get; }
        public bool BoolValue { get; }
        public float FloatValue { get; }
        public int IntValue { get; }
        public string StringValue { get; }
        public Vector3 VectorValue { get; }
    }
}