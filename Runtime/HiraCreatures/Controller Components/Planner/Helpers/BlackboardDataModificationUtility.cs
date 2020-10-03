using System.Collections.Generic;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace UnityEngine
{
    public static class BlackboardDataModificationUtility
    {
        public static void ApplyTo(this IEnumerable<IBlackboardModification> modifications,
            IReadWriteBlackboardDataSet dataSet)
        {
            foreach (var modification in modifications) modification.Apply(dataSet);
        }
    }
}