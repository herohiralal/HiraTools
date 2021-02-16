using UnityEngine.Events;

namespace UnityEngine.Internal
{
    [AddComponentMenu("HiraTools/ScriptableObject Variables/Listeners/Quaternion Listener")]
    public class QuaternionListenerComponent : ScriptableObjectVariableListenerComponent<Quaternion>
    {
        [SerializeField] private QuaternionVariable variable = null;
        [SerializeField] private QuaternionEvent onChange = null;
        protected override ScriptableObjectVariable<Quaternion> Variable => variable;
        protected override UnityEvent<Quaternion> OnChange => onChange;
    }
}