using HiraEngine.Components.Blackboard.Internal;
using UnityEngine;

namespace HiraEngine.Components.Blackboard
{
	public class BooleanKey : BlackboardKey<bool>
	{
#if UNITY_EDITOR
        public override unsafe void DrawEditor(void* data, IBlackboardComponent blackboard)
        {
            var value = *(byte*) data;
            var output = UnityEditor.EditorGUILayout.Toggle(name, (value).ToBoolean()).ToByte();
            if (output != value) MainThreadDispatcher.Schedule(() => blackboard.SetValue(Index, output));
        }
#endif
    }
}