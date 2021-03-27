using System;
using System.Collections.Generic;
using UnityEngine;

namespace HiraEngine.Components.AI.Internal
{
    public class SequenceExecutable : Executable, IPoolable
    {
        public SequenceExecutable Init(IEnumerable<IExecutableProvider> executableProviders, GameObject target, IBlackboardComponent blackboard)
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
                    _children.Dequeue().OnExecutionSuccess();
                    if (_children.Count == 0) return ExecutionStatus.Succeeded;
                    else
                    {
                        _children.Peek().OnExecutionStart();
                        return ExecutionStatus.InProgress;
                    }
                case ExecutionStatus.Failed:
                    _children.Dequeue().OnExecutionFailure();
                    return ExecutionStatus.Failed;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void OnExecutionSuccess() => GenericPool<SequenceExecutable>.Return(this);

        public override void OnExecutionFailure() => GenericPool<SequenceExecutable>.Return(this);

        public override void OnExecutionAbort()
        {
            _children.Dequeue().OnExecutionAbort();
            GenericPool<SequenceExecutable>.Return(this);
        }

        public void OnRetrieve()
        {
        }

        public void OnReturn()
        {
            _children.Clear();
        }
    }
}