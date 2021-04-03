using HiraEngine.Components.Blackboard.Internal;
using UnityEngine;

namespace HiraEngine.Components.Blackboard
{
    public class VectorKey : BlackboardKey<Vector3>
    {
#if UNITY_EDITOR
        public override unsafe void DrawEditor(void* data, IBlackboardComponent blackboard)
        {
            var valuePtr = (float*) data;
            var output = UnityEditor.EditorGUILayout.Vector3Field(name, new Vector3(valuePtr[0], valuePtr[1], valuePtr[2]));
            // ReSharper disable thrice CompareOfFloatsByEqualityOperator
            if (output.x != valuePtr[0] || output.y != valuePtr[1] || output.z != valuePtr[2])
            {
                MainThreadDispatcher.Schedule(() => blackboard.SetValue(Index, output));
            }
        }
#endif
    }
}