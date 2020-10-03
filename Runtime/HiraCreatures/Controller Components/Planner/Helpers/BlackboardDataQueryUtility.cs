// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace UnityEngine
{
    public static class BlackboardDataQueryUtility
    {
        
        // .DoesNotSatisfy(value)
        public static bool DoesNotSatisfy(this IReadOnlyBlackboardDataSet dataSet, IBlackboardQuery value) =>
            !value.IsSatisfiedBy(dataSet);
        
        // values.IsSatisfiedBy
        public static bool IsSatisfiedBy(this IBlackboardQuery[] values, IReadOnlyBlackboardDataSet dataSet)
        {
            var length = values.Length;
            for (var i = 0; i < length; i++)
            {
                if (!values[i].IsSatisfiedBy(dataSet)) return false;
            }
            return true;
        }

        // values.IsNotSatisfiedBy
        public static bool IsNotSatisfiedBy(this IBlackboardQuery[] values,IReadOnlyBlackboardDataSet dataSet)
        {
            var length = values.Length;
            for (var i = 0; i < length; i++)
            {
                if (!values[i].IsSatisfiedBy(dataSet)) return true;
            }
            return false;
        }
    }
}