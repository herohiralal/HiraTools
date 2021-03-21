using HiraEngine.Components.Blackboard.Internal;

namespace HiraEngine.Components.Blackboard
{
    public class IntegerKey : BlackboardKey<int>, INumericalBlackboardKey
    {
#if UNITY_EDITOR
        public override unsafe void DrawEditor(void* data, UnityEngine.IBlackboardComponent blackboard)
        {
            var value = *(int*) data;
            var output = UnityEditor.EditorGUILayout.DelayedIntField(name, value);
            if (value != output) blackboard.SetValue(Index, output);
        }
#endif
    }
}