using System;
using Hiralal.SOVariables.Core;

namespace UnityEngine
{
    [Serializable]
    public class BoolReference : ScriptableObjectVariableReference<bool>
    {
        public BoolReference() { }
        public BoolReference(bool value) : base(value) { }

        [SerializeField] private BoolVariable variable = null;
        public override ScriptableObjectVariable<bool> Variable => variable;
    }
}