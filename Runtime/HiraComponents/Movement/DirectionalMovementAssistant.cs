using UnityEngine;

namespace HiraEngine.Components.Movement.Internal
{
    public class DirectionalMovementAssistant
    {
        public Rigidbody Rigidbody { private get; set; } = null;
        public Transform Transform { private get; set; } = null;

        public float Speed { private get; set; } = 0f;
        public float Gravity { private get; set; } = 0f;
        public float FallMultiplier { private get; set; } = 0f;
        public float AngularSpeed { private get; set; } = 0f;

        public float RaycastSpaceRadius { private get; set; } = 0.25f;
        public float RaycastStartHeight { private get; set; } = 1f;
        public float RaycastLength { private get; set; } = 1.5f;
        public LayerMask FloorMask { private get; set; } = default;

        private readonly Vector3[] _floorRaycastHitPoints = new Vector3[5];
        
        private Vector3 _effectiveGravity = Vector3.zero;
        private float _timeSinceFalling = 0f;

        public void MoveTowards(in Vector3 direction)
        {
            CalculateFloorDistances();

            if (_floorRaycastHitPoints[0] == Vector3.zero)
            {
                _timeSinceFalling += Time.fixedDeltaTime;
                _effectiveGravity -=
                    (Gravity + (_timeSinceFalling * FallMultiplier)) * Time.fixedDeltaTime * Vector3.up;
            }

            Rigidbody.velocity = (Speed * direction) + _effectiveGravity;

            var floorAverage = FloorAverage;

            var rigidbodyPosition = Rigidbody.position;
            var floorPosition = new Vector3(rigidbodyPosition.x, floorAverage, rigidbodyPosition.z);

            if (_floorRaycastHitPoints[0] != Vector3.zero && Mathf.Abs(rigidbodyPosition.y - floorAverage) > 0.01f)
            {
                Rigidbody.MovePosition(floorPosition);
                _effectiveGravity = Vector3.zero;
                _timeSinceFalling = 0f;
            }
        }

        public void RotateTo(in Vector3 direction)
        {
            var currentYaw = Transform.eulerAngles.y.AsDegrees360();
            var targetYaw = Quaternion.LookRotation(direction).eulerAngles.y.AsDegrees360();

            currentYaw = (targetYaw - currentYaw).AsDegrees360() < 180
                ? (currentYaw + Mathf.Min(AngularSpeed * Time.deltaTime, (targetYaw - currentYaw).AsDegrees360()))
                .AsDegrees360()
                : (currentYaw - Mathf.Min(AngularSpeed * Time.deltaTime, (currentYaw - targetYaw).AsDegrees360()))
                .AsDegrees360();

            Transform.rotation = Quaternion.Euler(0, currentYaw, 0);
        }

        private void CalculateFloorDistances()
        {
            var right = Transform.right;
            var up = Transform.up;
            var forward = Transform.forward;
            _floorRaycastHitPoints[0] = FloorRaycast(Vector3.zero, in up);
            _floorRaycastHitPoints[1] = FloorRaycast(forward, in up);
            _floorRaycastHitPoints[2] = FloorRaycast(-forward, in up);
            _floorRaycastHitPoints[3] = FloorRaycast(right, in up);
            _floorRaycastHitPoints[4] = FloorRaycast(-right, in up);
        }

        private float FloorAverage
        {
            get
            {
                var floorAverage = 0f;
                var positiveRaycastsCount = 0;

                foreach (var floorRaycastHitPoint in _floorRaycastHitPoints)
                {
                    if (floorRaycastHitPoint == Vector3.zero) continue;

                    floorAverage += floorRaycastHitPoint.y;
                    positiveRaycastsCount++;
                }

                return floorAverage / positiveRaycastsCount;
            }
        }

        private Vector3 FloorRaycast(Vector3 offset, in Vector3 upDirection)
        {
            var raycastStartPoint = Transform.position + // position
                                    (upDirection * RaycastStartHeight) + // start height
                                    (offset * RaycastSpaceRadius); // radius

            var raycastEndPoint = raycastStartPoint - (upDirection * RaycastLength); // down stroke

            return Physics.Linecast(raycastStartPoint, raycastEndPoint, out var hit, FloorMask)
                ? hit.point
                : Vector3.zero;
        }

        public void DrawGizmos()
        {
#if DRAW_CHARACTER_MOVEMENT
            Gizmos.color = Color.magenta;

            var startPoint = Transform.position + Transform.up * RaycastStartHeight;
            var endPoint = startPoint + Vector3.down * RaycastLength;
            Gizmos.DrawLine(startPoint, endPoint); // center

            var forward = Transform.forward;
            var right = Transform.right;

            var modifier = forward * RaycastSpaceRadius;
            Gizmos.DrawLine(startPoint + modifier, endPoint + modifier); // forward
            modifier = (-forward) * RaycastSpaceRadius;
            Gizmos.DrawLine(startPoint + modifier, endPoint + modifier); // backward
            modifier = right * RaycastSpaceRadius;
            Gizmos.DrawLine(startPoint + modifier, endPoint + modifier); // right
            modifier = (-right) * RaycastSpaceRadius;
            Gizmos.DrawLine(startPoint + modifier, endPoint + modifier); // left
#endif
        }
    }
}