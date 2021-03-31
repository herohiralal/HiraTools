using System;
using System.Collections.Generic;
using UnityEngine;

namespace HiraEngine.Components.AI.Internal
{
    public class SequenceExecutable : Executable, IPoolReturnCallbackReceiver
    {
        public SequenceExecutable Init(IEnumerable<IExecutableProvider> executableProviders, HiraComponentContainer target, IBlackboardComponent blackboard)
        {
            foreach (var executableProvider in executableProviders)
                _children.Enqueue(executableProvider.GetExecutable(target, blackboard));

            return this;
        }

        private readonly Queue<Executable> _children = new Queue<Executable>();

        public override void OnExecutionStart() => _children.Peek().OnExecutionStart();

        public override ExecutionStatus Execute(float deltaTime)
        {
            var status = _children.Peek().Execute(deltaTime);
            switch (status)
            {
                case ExecutionStatus.InProgress:
                    return ExecutionStatus.InProgress;
                case ExecutionStatus.Succeeded:
                    _children.Peek().OnExecutionSuccess();
                    _children.Dequeue().Dispose();
                    if (_children.Count == 0) return ExecutionStatus.Succeeded;
                    else
                    {
                        _children.Peek().OnExecutionStart();
                        return ExecutionStatus.InProgress;
                    }
                case ExecutionStatus.Failed:
                    _children.Peek().OnExecutionFailure();
                    _children.Dequeue().Dispose();
                    return ExecutionStatus.Failed;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void OnExecutionAbort()
        {
            _children.Peek().OnExecutionAbort();
            _children.Dequeue().Dispose();
        }

        public override void Dispose() => GenericPool<SequenceExecutable>.Return(this);

        public void OnReturn() => _children.Clear();
    }
}