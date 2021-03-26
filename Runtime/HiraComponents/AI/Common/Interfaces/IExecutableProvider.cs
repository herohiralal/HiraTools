using UnityEngine;

namespace HiraEngine.Components.AI
{
    public interface IExecutableProvider
    {
        IExecutable GetExecutable(GameObject target, IBlackboardComponent blackboard);
    }
}