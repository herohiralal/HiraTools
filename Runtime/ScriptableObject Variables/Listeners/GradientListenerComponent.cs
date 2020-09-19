using UnityEngine;
using UnityEngine.Events;
using HiraEngine.SOVariables.Core;

namespace HiraEngine.SOVariables.ListenerComponents
{
    [AddComponentMenu("HiraTools/ScriptableObject Variables/Listeners/Gradient Listener")]
    public class GradientListenerComponent : ScriptableObjectVariableListenerComponent<Gradient>
    {
        [SerializeField] private GradientVariable variable = null;
        [SerializeField] private GradientEvent onChange = null;
        protected override ScriptableObjectVariable<Gradient> Variable => variable;
        protected override UnityEvent<Gradient> OnChange => onChange;
    }
}