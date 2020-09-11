using System;
using UnityEngine;
using UnityEngine.AI;

namespace HiraCreatures.Components.Movement
{
    public class HiraCreatureMover : HiraCreatureComponent, IHiraCreatureMover
    {
        [Space] [Header("Required Components")] [SerializeField]
        private Rigidbody targetRigidbody = null;
        [SerializeField] private NavMeshAgent targetNavMeshAgent = null;

        [Space] [Header("Movement Properties")] [SerializeField]
        private HiraCreatureMovementMode movementMode = HiraCreatureMovementMode.None;
        [SerializeField] private float directionalMovementSpeed = 4;
        [SerializeField] private float directionalMovementAngularSpeed = 360;
        [SerializeField] private float gravity = 9.8f;
        [SerializeField] private float fallMultiplier = 0.1f;

        [Space] [Header("Floor Detection")] [SerializeField]
        private float raycastSpaceRadius = 0.25f;
        [SerializeField] private float raycastStartHeight = 1f;
        [SerializeField] private float raycastLength = 1.5f;
        [SerializeField] private LayerMaskReference floorMask = null;

        private DirectionalMovementAssistant _directionalMovementAssistant = null;

        private void OnValidate()
        {
            if (_directionalMovementAssistant != null)
                UpdateDirectionalMovementAssistant();
        }

        public void MoveTowards(Vector3 direction)
        {
            _directionalMovementAssistant.MoveTowards(in direction);
            if (direction != Vector3.zero) _directionalMovementAssistant.RotateTo(in direction);
        }

        public void MoveTo(Vector3 position) => targetNavMeshAgent.SetDestination(position);

        public HiraCreatureMovementMode MovementMode
        {
            get => movementMode;
            set
            {
                if (movementMode == value) return;

                switch (movementMode)
                {
                    case HiraCreatureMovementMode.None: break;
                    case HiraCreatureMovementMode.Positional:
                        targetNavMeshAgent.isStopped = true;
                        targetNavMeshAgent.enabled = false;
                        break;
                    case HiraCreatureMovementMode.Directional:
                        _directionalMovementAssistant = null;
                        targetRigidbody.isKinematic = true;
                        break;
                    default: throw new ArgumentOutOfRangeException(nameof(movementMode), movementMode, null);
                }

                switch (value)
                {
                    case HiraCreatureMovementMode.None: break;
                    case HiraCreatureMovementMode.Positional:
                        targetNavMeshAgent.enabled = true;
                        targetNavMeshAgent.isStopped = false;
                        break;
                    case HiraCreatureMovementMode.Directional:
                        _directionalMovementAssistant = new DirectionalMovementAssistant();
                        targetRigidbody.isKinematic = false;
                        UpdateDirectionalMovementAssistant();
                        break;
                    default: throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }

                movementMode = value;
            }
        }

        private void UpdateDirectionalMovementAssistant()
        {
            _directionalMovementAssistant.Rigidbody = targetRigidbody;
            _directionalMovementAssistant.Transform = targetRigidbody.transform;

            _directionalMovementAssistant.Speed = directionalMovementSpeed;
            _directionalMovementAssistant.Gravity = gravity;
            _directionalMovementAssistant.FallMultiplier = fallMultiplier;
            _directionalMovementAssistant.AngularSpeed = directionalMovementAngularSpeed;

            _directionalMovementAssistant.RaycastSpaceRadius = raycastSpaceRadius;
            _directionalMovementAssistant.RaycastStartHeight = raycastStartHeight;
            _directionalMovementAssistant.RaycastLength = raycastLength;
            _directionalMovementAssistant.FloorMask = floorMask.Value;
        }

        private void OnDrawGizmos() => _directionalMovementAssistant?.DrawGizmos();
    }
}