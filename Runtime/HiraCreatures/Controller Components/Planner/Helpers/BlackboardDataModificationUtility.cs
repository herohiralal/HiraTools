// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace UnityEngine
{
    public static class BlackboardDataModificationUtility
    {
        public static void ApplyTo(this IBlackboardModification[] modifications,
            IReadWriteBlackboardDataSet dataSet)
        {
            var length = modifications.Length;
            for (var i = 0; i < length; i++) modifications[i].ApplyTo(dataSet);
        }
    }
}