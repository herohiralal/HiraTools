using HiraEngine.Components.Blackboard;
using UnityEngine;

namespace HiraEngine.Components.AI.Internal
{
    public class MoveToExecutableProvider : ScriptableObject, IExecutableProvider
    {
        [HiraCollectionDropdown(typeof(VectorKey))]
        [SerializeField] private HiraBlackboardKey targetPosition = null;

        [SerializeField] private float tolerance = 0.1f;

        public Executable GetExecutable(GameObject target, IBlackboardComponent blackboard)
        {
            var movementComponent = target.GetComponentInChildren<IMovementComponent>();
            return movementComponent != null
                ? GenericPool<StaticMoveToExecutable>.Retrieve().Init(movementComponent, blackboard, targetPosition, tolerance)
                : (Executable) AutoFailExecutable.INSTANCE;
        }
    }
}