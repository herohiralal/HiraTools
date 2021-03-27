using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

namespace HiraEngine.Components.AI.Internal
{
    public class StaticMoveToExecutable : Executable, IPoolable
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public StaticMoveToExecutable Init(IMovementComponent movementComponent, IBlackboardComponent blackboard, HiraBlackboardKey targetPositionKey, float tolerance)
        {
            _mover = movementComponent;
            _blackboard = blackboard;
            _targetPositionKey = targetPositionKey.Index;
            _tolerance = tolerance;
            return this;
        }
        
        private IMovementComponent _mover;
        private IBlackboardComponent _blackboard;
        private ushort _targetPositionKey;
        private Vector3 _targetPosition;
        private bool _hasFailed;
        private float _tolerance;

        public override void OnExecutionStart()
        {
            var targetPosition = _blackboard.GetValue<Vector3>(_targetPositionKey);

            if (NavMesh.SamplePosition(targetPosition, out var hit, _tolerance, NavMesh.AllAreas))
            {
                _targetPosition = hit.position;
                _mover.MovementMode = HiraCreatureMovementMode.Positional;
                _mover.MoveTo(_targetPosition, _tolerance);
            }
            else _hasFailed = true;

            _blackboard = null;
        }

        public override ExecutionStatus Execute(float deltaTime) =>
            _hasFailed
                ? ExecutionStatus.Failed
                : _mover.RemainingDistance < _tolerance
                    ? ExecutionStatus.Succeeded
                    : ExecutionStatus.InProgress;

        public override void OnExecutionAbort()
        {
	        _mover.StopMovingToDestination();
	        GenericPool<StaticMoveToExecutable>.Return(this);
        }

        public override void OnExecutionFailure() => GenericPool<StaticMoveToExecutable>.Return(this);

        public override void OnExecutionSuccess() => GenericPool<StaticMoveToExecutable>.Return(this);

        public void OnRetrieve()
        {
        }

        public void OnReturn()
        {
            _mover = null;
            _hasFailed = false;
        }
    }
}