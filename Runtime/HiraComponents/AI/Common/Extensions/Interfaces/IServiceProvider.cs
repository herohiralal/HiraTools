using UnityEngine;

namespace HiraEngine.Components.AI
{
    public interface IServiceProvider
    {
        Service GetService(HiraComponentContainer target, IBlackboardComponent blackboard);
    }
}