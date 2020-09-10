using UnityEngine;
using UnityEngine.Events;
using Hiralal.SOVariables.Core;

namespace Hiralal.SOVariables.ListenerComponents
{
    [AddComponentMenu("HiraTools/ScriptableObject Variables/Listeners/Color Listener")]
    public class ColorListenerComponent : ScriptableObjectVariableListenerComponent<Color>
    {
        [SerializeField] private ColorVariable variable = null;
        [SerializeField] private ColorEvent onChange = null;
        protected override ScriptableObjectVariable<Color> Variable => variable;
        protected override UnityEvent<Color> OnChange => onChange;
    }
}