using HiraEngine.Components.Blackboard.Internal;
using UnityEngine;

namespace HiraEngine.Components.Blackboard
{
    public class IntegerKey : BlackboardKey<int>, INumericalBlackboardKey
    {
#if UNITY_EDITOR
        public override unsafe void DrawEditor(void* data, IBlackboardComponent blackboard)
        {
            var value = *(int*) data;
            var output = UnityEditor.EditorGUILayout.DelayedIntField(name, value);
            if (value != output) MainThreadDispatcher.Schedule(() => blackboard.SetValue(Index, output));
        }
#endif
    }
}