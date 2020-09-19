using UnityEngine;
using UnityEngine.Events;
using HiraEngine.SOVariables.Core;

namespace HiraEngine.SOVariables.ListenerComponents
{
    [AddComponentMenu("HiraTools/ScriptableObject Variables/Listeners/LayerMask Listener")]
    public class LayerMaskListenerComponent : ScriptableObjectVariableListenerComponent<LayerMask>
    {
        [SerializeField] private LayerMaskVariable variable = null;
        [SerializeField] private LayerMaskEvent onChange = null;
        protected override ScriptableObjectVariable<LayerMask> Variable => variable;
        protected override UnityEvent<LayerMask> OnChange => onChange;
    }
}