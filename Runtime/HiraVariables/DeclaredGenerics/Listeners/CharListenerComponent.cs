using UnityEngine.Events;

namespace UnityEngine.Internal
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