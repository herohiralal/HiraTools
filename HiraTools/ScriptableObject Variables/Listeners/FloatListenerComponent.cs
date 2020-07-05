using UnityEngine;
using UnityEngine.Events;
using Hiralal.SOVariables.Core;

namespace Hiralal.SOVariables.ListenerComponents
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