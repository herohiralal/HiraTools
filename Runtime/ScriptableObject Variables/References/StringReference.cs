using System;
using HiraEngine.SOVariables.Core;

namespace UnityEngine
{
    [Serializable]
    public class StringReference : ScriptableObjectVariableReference<string>
    {
        public StringReference() { }
        public StringReference(string value) : base(value) { }

        [SerializeField] private StringVariable variable = null;
        public override ScriptableObjectVariable<string> Variable => variable;
    }
}