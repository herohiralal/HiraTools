﻿using HiraEngine.Components.Planner.Internal;
using NUnit.Framework;
using UnityEngine;
using static UnityEngine.BlackboardTypes;
using Assert = UnityEngine.Assertions.Assert;

namespace HiraTests.HiraEngine.Components.Planner
{
    [TestFixture]
    public class BoolValues
    {
        private static IReadWriteBlackboardDataSet GetDataSetWithValue(uint index, bool value)
        {
            var dataSet = GetWriteableDataSet(boolKeyCount: index + 1);
            dataSet.Booleans[index] = value;
            return dataSet;
        }
        
        [Test]
        public void bool_equals_value([Random(0u, 10u, 3)] uint index, [Values(false, true)] bool value)
        {
            var boolEqualsValue = new BoolEqualsValue(index, value);
            var correctDataSet = GetDataSetWithValue(index, value);
            var incorrectDataSet = GetDataSetWithValue(index, !value);
            
            Assert.IsTrue(boolEqualsValue.IsSatisfiedBy(correctDataSet));
            Assert.IsFalse(boolEqualsValue.IsSatisfiedBy(incorrectDataSet));
        }
    }
}