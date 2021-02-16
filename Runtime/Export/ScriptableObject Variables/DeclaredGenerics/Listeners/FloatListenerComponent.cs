using UnityEngine.Events;

namespace UnityEngine.Internal
{
    [AddComponentMenu("HiraTools/ScriptableObject Variables/Listeners/Float Listener")]
    public class FloatListenerComponent : ScriptableObjectVariableListenerComponent<float>
    {
        [SerializeField] private FloatVariable variable = null;
        [SerializeField] private FloatEvent onChange = null;
        protected override ScriptableObjectVariable<float> Variable => variable;
        protected override UnityEvent<float> OnChange => onChange;
    }
}