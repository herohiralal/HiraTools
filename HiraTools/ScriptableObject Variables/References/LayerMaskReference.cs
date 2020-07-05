using System;
using UnityEngine;
using Hiralal.SOVariables.Core;

namespace UnityEngine
{
    [Serializable]
    public class LayerMaskReference : ScriptableObjectVariableReference<LayerMask>
    {
        public LayerMaskReference() { }
        public LayerMaskReference(LayerMask value) : base(value) { }

        [SerializeField] private LayerMaskVariable variable = null;
        public override ScriptableObjectVariable<LayerMask> Variable => variable;
    }
}