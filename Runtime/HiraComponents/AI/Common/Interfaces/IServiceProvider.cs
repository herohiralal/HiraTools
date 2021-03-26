using UnityEngine;

namespace HiraEngine.Components.AI
{
    public interface IServiceProvider
    {
        IService GetService(GameObject target, IBlackboardComponent blackboard);
    }
}