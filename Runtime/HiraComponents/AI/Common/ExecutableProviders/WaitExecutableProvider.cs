using UnityEngine;

namespace HiraEngine.Components.AI.Internal
{
    public class WaitExecutableProvider : ScriptableObject, IExecutableProvider
    {
        [SerializeField] private float time = 1f;

        public Executable GetExecutable(GameObject target, IBlackboardComponent blackboard) =>
            GenericPool<WaitExecutable>.Retrieve().Init(time);
    }
}