using UnityEngine;

namespace HiraEngine.Components.Movement
{
    public interface IDirectionalMovementAssistant
    {
        Rigidbody Rigidbody { set; }
        Transform Transform { set; }
        float Speed { set; }
        float Gravity { set; }
        float FallMultiplier { set; }
        float AngularSpeed { set; }
        float RaycastSpaceRadius { set; }
        float RaycastStartHeight { set; }
        float RaycastLength { set; }
        LayerMask FloorMask { set; }
        void MoveTowards(in Vector3 direction);
        void RotateTo(in Vector3 direction);
        void DrawGizmos();
    }
}