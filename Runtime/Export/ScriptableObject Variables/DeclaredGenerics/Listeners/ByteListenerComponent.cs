using UnityEngine.Events;

namespace UnityEngine.Internal
{
    [AddComponentMenu("HiraTools/ScriptableObject Variables/Listeners/Byte Listener")]
    public class ByteListenerComponent : ScriptableObjectVariableListenerComponent<byte>
    {
        [SerializeField] private ByteVariable variable = null;
        [SerializeField] private ByteEvent onChange = null;
        protected override ScriptableObjectVariable<byte> Variable => variable;
        protected override UnityEvent<byte> OnChange => onChange;
    }
}