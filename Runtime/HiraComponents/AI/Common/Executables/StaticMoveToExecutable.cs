using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

namespace HiraEngine.Components.AI.Internal
{
    public class StaticMoveToExecutable : Executable, IPoolReturnCallbackReceiver
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public StaticMoveToExecutable Init(NavMeshAgent navMeshAgent, IBlackboardComponent blackboard, HiraBlackboardKey targetPositionKey, bool followTarget, float speed, float tolerance)
        {
	        _navMeshAgent = navMeshAgent;
            _blackboard = blackboard;
            _targetPositionKey = targetPositionKey.Index;
            _followTarget = followTarget;
            _speed = speed;
            _tolerance = tolerance;
            return this;
        }
        
        private NavMeshAgent _navMeshAgent;
        private readonly NavMeshPath _path = new NavMeshPath();
        private IBlackboardComponent _blackboard;
        private ushort _targetPositionKey;
        private bool _followTarget;
        private bool _hasFailed;
        private float _speed;
        private float _tolerance;

        private float _timeSinceDestinationUpdate = 0f;

        public override void OnExecutionStart()
        {
	        var targetPosition = _blackboard.GetValue<Vector3>(_targetPositionKey);

	        UpdateNavMeshAgentDestination(targetPosition);
        }

        public override ExecutionStatus Execute(float deltaTime)
        {
	        if (_followTarget && !_hasFailed && _timeSinceDestinationUpdate > 1f)
	        {
		        _timeSinceDestinationUpdate += deltaTime;

		        var targetPosition = _blackboard.GetValue<Vector3>(_targetPositionKey);
		        if (_navMeshAgent.destination != targetPosition)
		        {
			        UpdateNavMeshAgentDestination(targetPosition);
		        }
	        }

	        return _hasFailed
		        ? ExecutionStatus.Failed
		        : _navMeshAgent.remainingDistance < _tolerance
			        ? ExecutionStatus.Succeeded
			        : ExecutionStatus.InProgress;
        }

        private void UpdateNavMeshAgentDestination(Vector3 targetPosition)
        {
	        if (NavMesh.SamplePosition(targetPosition, out var hit, _tolerance, NavMesh.AllAreas))
	        {
		        targetPosition = hit.position;
	        }
	        else
	        {
		        _hasFailed = true;
		        return;
	        }
	        
	        _navMeshAgent.CalculatePath(targetPosition, _path);
	        if (_path.status == NavMeshPathStatus.PathComplete)
	        {
		        _navMeshAgent.SetPath(_path);
		        _navMeshAgent.speed = _speed;
		        _navMeshAgent.stoppingDistance = _tolerance;
		        _navMeshAgent.isStopped = false;
	        }
	        else _hasFailed = true;

	        _timeSinceDestinationUpdate = 0f;
        }

        public override void OnExecutionAbort() => _navMeshAgent.isStopped = true;

        public override void Dispose() => GenericPool<StaticMoveToExecutable>.Return(this);

        public void OnReturn()
        {
	        _blackboard = null;
	        _navMeshAgent = null;
            _hasFailed = false;
        }
    }
}