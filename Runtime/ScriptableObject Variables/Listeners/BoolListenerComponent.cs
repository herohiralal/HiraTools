using UnityEngine;
using UnityEngine.Events;
using HiraEngine.SOVariables.Core;

namespace HiraEngine.SOVariables.ListenerComponents
{
    [AddComponentMenu("HiraTools/ScriptableObject Variables/Listeners/Bool Listener")]
    public class BoolListenerComponent : ScriptableObjectVariableListenerComponent<bool>
    {
        [SerializeField] private BoolVariable variable = null;
        [SerializeField] private BoolEvent onChange = null;
        protected override ScriptableObjectVariable<bool> Variable => variable;
        protected override UnityEvent<bool> OnChange => onChange;
    }
}