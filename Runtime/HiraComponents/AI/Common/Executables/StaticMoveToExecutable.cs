using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

namespace HiraEngine.Components.AI.Internal
{
    public class StaticMoveToExecutable : Executable, IPoolable
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public StaticMoveToExecutable Init(NavMeshAgent navMeshAgent, IBlackboardComponent blackboard, HiraBlackboardKey targetPositionKey, float tolerance)
        {
	        _navMeshAgent = navMeshAgent;
            _blackboard = blackboard;
            _targetPositionKey = targetPositionKey.Index;
            _tolerance = tolerance;
            return this;
        }
        
        private NavMeshAgent _navMeshAgent;
        private readonly NavMeshPath _path = new NavMeshPath();
        private IBlackboardComponent _blackboard;
        private ushort _targetPositionKey;
        private bool _hasFailed;
        private float _tolerance;

        public override void OnExecutionStart()
        {
            var targetPosition = _blackboard.GetValue<Vector3>(_targetPositionKey);

            _navMeshAgent.CalculatePath(targetPosition, _path);
            if (_path.status == NavMeshPathStatus.PathComplete)
            {
	            _navMeshAgent.SetPath(_path);
	            _navMeshAgent.stoppingDistance = _tolerance;
	            _navMeshAgent.isStopped = false;
            }
            else _hasFailed = true;

            _blackboard = null;
        }

        public override ExecutionStatus Execute(float deltaTime) =>
            _hasFailed
                ? ExecutionStatus.Failed
                : _navMeshAgent.remainingDistance < _tolerance
                    ? ExecutionStatus.Succeeded
                    : ExecutionStatus.InProgress;

        public override void OnExecutionAbort()
        {
	        _navMeshAgent.isStopped = true;
	        GenericPool<StaticMoveToExecutable>.Return(this);
        }

        public override void OnExecutionFailure() => GenericPool<StaticMoveToExecutable>.Return(this);

        public override void OnExecutionSuccess() => GenericPool<StaticMoveToExecutable>.Return(this);

        public void OnRetrieve()
        {
        }

        public void OnReturn()
        {
	        _navMeshAgent = null;
            _hasFailed = false;
        }
    }
}