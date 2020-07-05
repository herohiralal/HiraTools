using System;
using UnityEngine;
using Hiralal.SOVariables.Core;

namespace UnityEngine
{
    [Serializable]
    public class Vector2Reference : ScriptableObjectVariableReference<Vector2>
    {
        public Vector2Reference() { }
        public Vector2Reference(Vector2 value) : base(value) { }

        [SerializeField] private Vector2Variable variable = null;
        public override ScriptableObjectVariable<Vector2> Variable => variable;
    }
}