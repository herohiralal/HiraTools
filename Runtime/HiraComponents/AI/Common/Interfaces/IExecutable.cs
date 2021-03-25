using UnityEngine;

namespace HiraEngine.Components.AI
{
    public enum ExecutionStatus
    {
        InProgress, Succeeded, Failed
    }
    
    public interface IExecutable
    {
        void OnExecutionStart();
        ExecutionStatus Execute(float deltaTime);
        void OnExecutionSuccess();
        void OnExecutionFailure();
        void OnExecutionAbort();
    }
}