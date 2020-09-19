using UnityEngine;
using UnityEngine.Events;
using HiraEngine.SOVariables.Core;

namespace HiraEngine.SOVariables.ListenerComponents
{
    [AddComponentMenu("HiraTools/ScriptableObject Variables/Listeners/Char Listener")]
    public class CharListenerComponent : ScriptableObjectVariableListenerComponent<char>
    {
        [SerializeField] private CharVariable variable = null;
        [SerializeField] private CharEvent onChange = null;
        protected override ScriptableObjectVariable<char> Variable => variable;
        protected override UnityEvent<char> OnChange => onChange;
    }
}