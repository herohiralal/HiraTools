using UnityEngine;
using UnityEngine.Events;
using Hiralal.SOVariables.Core;

namespace Hiralal.SOVariables.ListenerComponents
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