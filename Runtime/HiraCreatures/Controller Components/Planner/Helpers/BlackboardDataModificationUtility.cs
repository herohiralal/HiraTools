using System.Collections.Generic;
using HiraEngine.Components.Planner.Internal;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace UnityEngine
{
    public static class BlackboardDataModificationUtility
    {
        // Apply(value)
        public static IBlackboardModification Apply(this IReadWriteBlackboardDataSet dataSet,
            IBlackboardModification modification) =>
            modification.ApplyTo(dataSet);

        // Values.Apply
        public static UndoStack ApplyTo(
            this IReadOnlyList<IBlackboardModification> modifications, IReadWriteBlackboardDataSet dataSet)
        {
            var count = modifications.Count;
            var undoBuffer = new IBlackboardModification[count];
            for (var i = 0; i < count; i++) undoBuffer[i] = modifications[i].ApplyTo(dataSet);
            return new UndoStack(undoBuffer);
        }

        // Apply(values)
        public static UndoStack Apply(this IReadWriteBlackboardDataSet dataSet,
            IReadOnlyList<IBlackboardModification> modifications) =>
            modifications.ApplyTo(dataSet);

        public static void Apply(this IReadWriteBlackboardDataSet dataSet,
            UndoStack undoStack) =>
            undoStack.ApplyTo(dataSet);
    }
}