using System.Collections.Generic;
using System.Linq;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace UnityEngine
{
    public static class BlackboardDataQueryUtility
    {
        // .Satisfies(value)
        public static bool Satisfies(this IReadOnlyBlackboardDataSet dataSet, IBlackboardQuery value) => 
            value.IsSatisfiedBy(dataSet);

        public static bool Satisfies(this IBlackboardValueAccessor accessor, IBlackboardQuery value) =>
            value.IsSatisfiedBy(accessor.DataSet);
        
        // .Satisfies(values)
        public static bool Satisfies(this IReadOnlyBlackboardDataSet dataSet, IEnumerable<IBlackboardQuery> values) => 
            values.All(dataSet.Satisfies);

        public static bool Satisfies(this IBlackboardValueAccessor accessor, IEnumerable<IBlackboardQuery> values) => 
            values.All(accessor.DataSet.Satisfies);
        
        // value.IsSatisfiedBy
        public static bool IsSatisfiedBy(this IBlackboardQuery value, IBlackboardValueAccessor accessor) => 
            value.IsSatisfiedBy(accessor.DataSet);

        // values.IsSatisfiedBy
        public static bool IsSatisfiedBy(this IEnumerable<IBlackboardQuery> values,IReadOnlyBlackboardDataSet dataSet) => 
            values.All(dataSet.Satisfies);

        public static bool IsSatisfiedBy(this IEnumerable<IBlackboardQuery> values,IBlackboardValueAccessor accessor) => 
            values.All(accessor.DataSet.Satisfies);
        
        // .DoesNotSatisfy(value)
        public static bool DoesNotSatisfy(this IReadOnlyBlackboardDataSet dataSet, IBlackboardQuery value) =>
            !value.IsSatisfiedBy(dataSet);

        public static bool DoesNotSatisfy(this IBlackboardValueAccessor accessor, IBlackboardQuery value) =>
            !value.IsSatisfiedBy(accessor.DataSet);
        
        // .DoesNotSatisfy(values)
        public static bool DoesNotSatisfy(this IReadOnlyBlackboardDataSet dataSet, IEnumerable<IBlackboardQuery> values) => 
            !values.All(dataSet.Satisfies);

        public static bool DoesNotSatisfy(this IBlackboardValueAccessor accessor, IEnumerable<IBlackboardQuery> values) => 
            !values.All(accessor.DataSet.Satisfies);
        
        // value.IsNotSatisfiedBy
        public static bool IsNotSatisfiedBy(this IBlackboardQuery value, IReadOnlyBlackboardDataSet dataSet) =>
            !value.IsSatisfiedBy(dataSet);
        
        public static bool IsNotSatisfiedBy(this IBlackboardQuery value, IBlackboardValueAccessor accessor) => 
            !value.IsSatisfiedBy(accessor.DataSet);

        // values.IsNotSatisfiedBy
        public static bool IsNotSatisfiedBy(this IEnumerable<IBlackboardQuery> values,IReadOnlyBlackboardDataSet dataSet) => 
            !values.All(dataSet.Satisfies);

        public static bool IsNotSatisfiedBy(this IEnumerable<IBlackboardQuery> values,IBlackboardValueAccessor accessor) => 
            !values.All(accessor.DataSet.Satisfies);
    }
}