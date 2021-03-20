namespace HiraEngine.Components.Blackboard.Internal
{
	public class BooleanKey : BlackboardKey<bool>
	{
#if UNITY_EDITOR
        public override unsafe void DrawEditor(void* data, UnityEngine.IBlackboardComponent blackboard)
        {
            var value = *(byte*) data;
            var output = UnityEditor.EditorGUILayout.Toggle(name, (value).ToBoolean()).ToByte();
            if (output != value) blackboard.SetValue(Index, output);
        }
#endif
    }
}