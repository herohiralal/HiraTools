using HiraEngine.Components.Blackboard;
using UnityEngine;
using UnityEngine.AI;

namespace HiraEngine.Components.AI.Internal
{
    public class MoveToExecutableProvider : ScriptableObject, IExecutableProvider
    {
        [HiraCollectionDropdown(typeof(VectorKey))]
        [SerializeField] private HiraBlackboardKey targetPosition = null;

        [SerializeField] private float tolerance = 0.1f;

        public Executable GetExecutable(HiraComponentContainer target, IBlackboardComponent blackboard)
        {
	        if (target is IContainsComponent<NavMeshAgent> navigableTarget && navigableTarget.Component != null)
	        {
		        return GenericPool<StaticMoveToExecutable>.Retrieve().Init(navigableTarget.Component, blackboard, targetPosition, tolerance);
	        }
	        
	        return AutoFailExecutable.INSTANCE;
        }
    }
}