using UnityEngine.Events;

namespace UnityEngine.Internal
{
    [AddComponentMenu("HiraTools/ScriptableObject Variables/Listeners/Int Listener")]
    public class IntListenerComponent : ScriptableObjectVariableListenerComponent<int>
    {
        [SerializeField] private IntVariable variable = null;
        [SerializeField] private IntEvent onChange = null;
        protected override ScriptableObjectVariable<int> Variable => variable;
        protected override UnityEvent<int> OnChange => onChange;
    }
}