using UnityEngine;

namespace HiraEngine.Components.AI.Internal
{
    public class WaitExecutableProvider : ScriptableObject, IExecutableProvider
    {
        [SerializeField] private float time = 1f;

        public Executable GetExecutable(HiraComponentContainer target, IBlackboardComponent blackboard) =>
            GenericPool<WaitExecutable>.Retrieve().Init(time);

        private void OnValidate() => name = ToString();
        public override string ToString() => $"Wait for {time} seconds";
    }
}