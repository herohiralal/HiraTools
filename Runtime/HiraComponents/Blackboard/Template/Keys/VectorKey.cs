namespace HiraEngine.Components.Blackboard.Internal
{
    public class VectorKey : BlackboardKey<UnityEngine.Vector3>
    {
#if UNITY_EDITOR
        public override unsafe void DrawEditor(void* data, UnityEngine.IBlackboardComponent blackboard)
        {
            var valuePtr = (float*) data;
            var output = UnityEditor.EditorGUILayout.Vector3Field(name, new UnityEngine.Vector3(valuePtr[0], valuePtr[1], valuePtr[2]));
            // ReSharper disable thrice CompareOfFloatsByEqualityOperator
            if (output.x != valuePtr[0] || output.y != valuePtr[1] || output.z != valuePtr[2])
            {
                blackboard.SetValue(Index, output);
            }
        }
#endif
    }
}