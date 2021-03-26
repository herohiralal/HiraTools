using UnityEngine;

namespace HiraEngine.Components.AI
{
    public interface IExecutableProvider
    {
        Executable GetExecutable(GameObject target, IBlackboardComponent blackboard);
    }
}