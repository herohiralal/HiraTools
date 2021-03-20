namespace HiraEngine.Components.Blackboard.Internal
{
    public class FloatKey : BlackboardKey<float>, INumericalBlackboardKey
    {
#if UNITY_EDITOR
        public override unsafe void DrawEditor(void* data, UnityEngine.IBlackboardComponent blackboard)
        {
            var value = *(float*) data;
            var output = UnityEditor.EditorGUILayout.DelayedFloatField(name, value);

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (output != value) blackboard.SetValue(Index, output);
        }
#endif
    }
}