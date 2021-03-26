using UnityEngine;

namespace HiraEngine.Components.AI
{
    public interface IServiceProvider
    {
        Service GetService(GameObject target, IBlackboardComponent blackboard);
    }
}