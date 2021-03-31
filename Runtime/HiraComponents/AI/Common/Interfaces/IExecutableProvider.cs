using UnityEngine;

namespace HiraEngine.Components.AI
{
    public interface IExecutableProvider
    {
        Executable GetExecutable(HiraComponentContainer target, IBlackboardComponent blackboard);
    }
}