﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace HiraCreatures.Components.Blackboard.Helpers
{
    public static class BlackboardDataValueChecker
    {
        // .Satisfies(value)
        public static bool Satisfies(this IReadOnlyBlackboardDataSet dataSet, IBlackboardValue value) => 
            value.IsSatisfiedBy(dataSet);

        public static bool Satisfies(this IBlackboardValueAccessor accessor, IBlackboardValue value) =>
            value.IsSatisfiedBy(accessor.DataSet);
        
        // .Satisfies(values)
        public static bool Satisfies(this IReadOnlyBlackboardDataSet dataSet, IEnumerable<IBlackboardValue> values) => 
            values.All(dataSet.Satisfies);

        public static bool Satisfies(this IBlackboardValueAccessor accessor, IEnumerable<IBlackboardValue> values) => 
            values.All(accessor.DataSet.Satisfies);
        
        // value.IsSatisfiedBy
        public static bool IsSatisfiedBy(this IBlackboardValue value, IBlackboardValueAccessor accessor) => 
            value.IsSatisfiedBy(accessor.DataSet);

        // values.IsSatisfiedBy
        public static bool IsSatisfiedBy(this IEnumerable<IBlackboardValue> values,IReadOnlyBlackboardDataSet dataSet) => 
            values.All(dataSet.Satisfies);

        public static bool IsSatisfiedBy(this IEnumerable<IBlackboardValue> values,IBlackboardValueAccessor accessor) => 
            values.All(accessor.DataSet.Satisfies);
    }
}