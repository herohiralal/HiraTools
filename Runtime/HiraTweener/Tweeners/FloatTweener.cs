using System;
using UnityEngine;
using UnityEngine.Events;

namespace HiraEngine.Tweeners
{
    [AddComponentMenu("HiraTools/Tweeners/Float Tweener")]
    public class FloatTweener : TweenerBase<float>
    {
        [Space] [Header("Core")]
        [SerializeField] private FloatEvent target = null;
        [SerializeField] private FloatReference startValue = null;
        [SerializeField] private FloatReference endValue = null;
        
        protected override UnityEvent<float> Target => target;
        protected override ScriptableObjectVariableReference<float> StartValue => startValue;
        protected override ScriptableObjectVariableReference<float> EndValue => endValue;
        protected override Func<float, float, float, float> InterpolationFunction => Mathf.LerpUnclamped;
    }
}