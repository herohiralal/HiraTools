using HiraEngine.Components.Blackboard.Internal;
using UnityEngine;

namespace HiraEngine.Components.Blackboard
{
    public class FloatKey : BlackboardKey<float>, INumericalBlackboardKey
    {
#if UNITY_EDITOR
        public override unsafe void DrawEditor(void* data, IBlackboardComponent blackboard)
        {
            var value = *(float*) data;
            var output = UnityEditor.EditorGUILayout.DelayedFloatField(name, value);

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (output != value) MainThreadDispatcher.Schedule(() => blackboard.SetValue(Index, output));
        }
#endif
    }
}