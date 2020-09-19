using UnityEngine;
using UnityEngine.Events;
using HiraEngine.SOVariables.Core;

namespace HiraEngine.SOVariables.ListenerComponents
{
    [AddComponentMenu("HiraTools/ScriptableObject Variables/Listeners/String Listener")]
    public class StringListenerComponent : ScriptableObjectVariableListenerComponent<string>
    {
        [SerializeField] private StringVariable variable = null;
        [SerializeField] private StringEvent onChange = null;
        protected override ScriptableObjectVariable<string> Variable => variable;
        protected override UnityEvent<string> OnChange => onChange;
    }
}