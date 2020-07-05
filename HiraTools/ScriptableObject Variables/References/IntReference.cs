using System;
using UnityEngine;
using Hiralal.SOVariables.Core;

namespace UnityEngine
{
    [Serializable]
    public class IntReference : ScriptableObjectVariableReference<int>
    {
        public IntReference() { }
        public IntReference(int value) : base(value) { }

        [SerializeField] private IntVariable variable = null;
        public override ScriptableObjectVariable<int> Variable => variable;
    }
}