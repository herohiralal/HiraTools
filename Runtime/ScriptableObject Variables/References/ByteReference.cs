using System;
using HiraEngine.SOVariables.Core;

namespace UnityEngine
{
    [Serializable]
    public class ByteReference : ScriptableObjectVariableReference<byte>
    {
        public ByteReference() { }
        public ByteReference(byte value) : base(value) { }

        [SerializeField] private ByteVariable variable = null;
        public override ScriptableObjectVariable<byte> Variable => variable;
    }
}