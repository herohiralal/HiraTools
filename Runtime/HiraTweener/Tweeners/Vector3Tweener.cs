using System;
using UnityEngine;
using UnityEngine.Events;

namespace HiraEngine.Tweeners
{
    [AddComponentMenu("HiraTools/Tweeners/Vector3 Tweener")]
    public class Vector3Tweener : TweenerBase<Vector3>
    {
        [Space] [Header("Core")]
        [SerializeField] private Vector3Event target = null;
        [SerializeField] private Vector3Reference startValue = null;
        [SerializeField] private Vector3Reference endValue = null;
        
        protected override UnityEvent<Vector3> Target => target;
        protected override ScriptableObjectVariableReference<Vector3> StartValue => startValue;
        protected override ScriptableObjectVariableReference<Vector3> EndValue => endValue;
        protected override Func<Vector3, Vector3, float, Vector3> InterpolationFunction => Vector3.LerpUnclamped;
    }
}