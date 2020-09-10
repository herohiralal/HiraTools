using System.Diagnostics;

namespace UnityEngine
{
    public class DirectionalMovementAssistant
    {
        public Rigidbody Rigidbody = null;
        public Transform Transform = null;

        public float Speed = 0f;
        public float Gravity = 0f;
        public float FallMultiplier = 0f;
        public float AngularSpeed = 0f;

        public float RaycastSpaceRadius = 0.25f;
        public float RaycastStartHeight = 1f;
        public float RaycastLength = 1.5f;
        public LayerMask FloorMask = default;

        // floor detection pre-allocated memory
        private readonly Vector3[] _floorRaycastHitPoints = new Vector3[5];
        private Vector3 _effectiveGravity = Vector3.zero;
        private Vector3 _rigidbodyPosition = Vector3.zero;
        private Vector3 _floorPosition = Vector3.zero;
        private RaycastHit _hit = default;
        private Vector3 _raycastStartPoint = Vector3.zero;
        private Vector3 _raycastEndPoint = Vector3.zero;
        private float _floorAverage = 0f;
        private int _positiveRaycastsCount = 0;
        private float _timeSinceFalling = 0f;

        // Rigidbody directions pre-allocated memory
        private Vector3 _right = Vector3.zero;
        private Vector3 _up = Vector3.zero;
        private Vector3 _forward = Vector3.zero;

        // Rotation pre-allocated memory
        private float _currentYaw = 0;
        private float _targetYaw = 0;

        public void MoveTowards(in Vector3 direction)
        {
            CalculateDirections();
            CalculateFloorDistances();

            if (_floorRaycastHitPoints[0] == Vector3.zero)
            {
                _timeSinceFalling += Time.fixedDeltaTime;
                _effectiveGravity -=
                    (Gravity + (_timeSinceFalling * FallMultiplier)) * Time.fixedDeltaTime * Vector3.up;
            }

            Rigidbody.velocity = (Speed * direction) + _effectiveGravity;

            UpdateFloorAverage();

            _rigidbodyPosition = Rigidbody.position;
            _floorPosition.x = _rigidbodyPosition.x;
            _floorPosition.y = _floorAverage;
            _floorPosition.z = _rigidbodyPosition.z;

            if (_floorRaycastHitPoints[0] != Vector3.zero && _floorPosition != Rigidbody.position)
            {
                Rigidbody.MovePosition(_floorPosition);
                _effectiveGravity = Vector3.zero;
                _timeSinceFalling = 0f;
            }
        }

        public void RotateTo(in Vector3 direction)
        {
            _currentYaw = Transform.eulerAngles.y.AsDegrees360();
            _targetYaw = Quaternion.LookRotation(direction).eulerAngles.y.AsDegrees360();

            if ((_targetYaw - _currentYaw).AsDegrees360() < 180)
            {
                _currentYaw = (_currentYaw + Mathf.Min(AngularSpeed * Time.deltaTime, (_targetYaw - _currentYaw).AsDegrees360()))
                    .AsDegrees360();
            }
            else
            {
                _currentYaw = (_currentYaw - Mathf.Min(AngularSpeed * Time.deltaTime, (_currentYaw - _targetYaw).AsDegrees360()))
                    .AsDegrees360();
            }

            Transform.rotation = Quaternion.Euler(0, _currentYaw, 0);
        }

        private void CalculateDirections()
        {
            _right = Transform.right;
            _up = Transform.up;
            _forward = Transform.forward;
        }

        private void CalculateFloorDistances()
        {
            _floorRaycastHitPoints[0] = FloorRaycast(Vector3.zero);
            _floorRaycastHitPoints[1] = FloorRaycast(_forward);
            _floorRaycastHitPoints[2] = FloorRaycast(-_forward);
            _floorRaycastHitPoints[3] = FloorRaycast(_right);
            _floorRaycastHitPoints[4] = FloorRaycast(-_right);
        }

        private void UpdateFloorAverage()
        {
            _floorAverage = 0;
            _positiveRaycastsCount = 0;

            foreach (var floorRaycastHitPoint in _floorRaycastHitPoints)
            {
                if (floorRaycastHitPoint == Vector3.zero) continue;

                _floorAverage += floorRaycastHitPoint.y;
                _positiveRaycastsCount++;
            }

            _floorAverage /= _positiveRaycastsCount;
        }

        private Vector3 FloorRaycast(Vector3 offset)
        {
            _up = Transform.up;

            _raycastStartPoint = Transform.position + // position
                                 (_up * RaycastStartHeight) + // start height
                                 (offset * RaycastSpaceRadius); // radius

            _raycastEndPoint = _raycastStartPoint - (_up * RaycastLength); // down stroke

            return Physics.Linecast(_raycastStartPoint, _raycastEndPoint, out _hit, FloorMask)
                ? _hit.point
                : Vector3.zero;
        }

        [Conditional("DRAW_CHARACTER_MOVEMENT")]
        public void DrawGizmos()
        {
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
        }
    }
}