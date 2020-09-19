using System;
using HiraEngine.SOVariables.Core;

namespace UnityEngine
{
    [Serializable]
    public class CharReference : ScriptableObjectVariableReference<char>
    {
        public CharReference() { }
        public CharReference(char value) : base(value) { }

        [SerializeField] private CharVariable variable = null;
        public override ScriptableObjectVariable<char> Variable => variable;
    }
}