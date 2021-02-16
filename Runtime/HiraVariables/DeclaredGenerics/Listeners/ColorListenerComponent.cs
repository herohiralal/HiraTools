using UnityEngine.Events;

namespace UnityEngine.Internal
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