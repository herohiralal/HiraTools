using HiraEngine.Components.Blackboard;
using UnityEngine;
using UnityEngine.AI;

namespace HiraEngine.Components.AI.Internal
{
    public class MoveToExecutableProvider : ScriptableObject, IExecutableProvider
    {
        [HiraCollectionDropdown(typeof(VectorKey))]
        [SerializeField] private HiraBlackboardKey targetPosition = null;

        [SerializeField] private bool followTarget = false;
        [SerializeField] private float speed = 3.5f;
        [SerializeField] private float tolerance = 0.1f;

        public Executable GetExecutable(HiraComponentContainer target, IBlackboardComponent blackboard)
        {
	        if (target is IContainsComponent<NavMeshAgent> navigableTarget && navigableTarget.Component != null)
	        {
		        return GenericPool<StaticMoveToExecutable>.Retrieve().Init(navigableTarget.Component, blackboard, targetPosition, followTarget, speed, tolerance);
	        }
	        
	        return AutoFailExecutable.INSTANCE;
        }

        private void OnValidate() => name = ToString();

        public override string ToString() =>
            targetPosition == null 
                ? "INVALID EXECUTABLE" 
                : $"Move to {targetPosition.name}";
    }
}