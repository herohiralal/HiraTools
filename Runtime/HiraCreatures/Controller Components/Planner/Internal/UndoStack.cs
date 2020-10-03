using System.Collections.Generic;
using UnityEngine;

namespace HiraEngine.Components.Planner.Internal
{
    public readonly struct UndoStack
    {
        // TODO: Get rid of this class.
        public UndoStack(IReadOnlyList<IBlackboardModification> stack) => _stack = stack;
        private readonly IReadOnlyList<IBlackboardModification> _stack;

        public void ApplyTo(IReadWriteBlackboardDataSet dataSet)
        {
            for (var count = _stack.Count - 1; count  > -1; count--)
            {
                _stack[count].ApplyTo(dataSet);
            }
        }
    }
}