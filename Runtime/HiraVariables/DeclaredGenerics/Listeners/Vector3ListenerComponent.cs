using UnityEngine.Events;

namespace UnityEngine.Internal
{
    [AddComponentMenu("HiraTools/ScriptableObject Variables/Listeners/Vector3 Listener")]
    public class Vector3ListenerComponent : ScriptableObjectVariableListenerComponent<Vector3>
    {
        [SerializeField] private Vector3Variable variable = null;
        [SerializeField] private Vector3Event onChange = null;
        protected override ScriptableObjectVariable<Vector3> Variable => variable;
        protected override UnityEvent<Vector3> OnChange => onChange;
    }
}