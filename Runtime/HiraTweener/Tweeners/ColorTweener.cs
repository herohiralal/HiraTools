using System;
using HiraEngine.SOVariables.Core;
using UnityEngine;
using UnityEngine.Events;

namespace HiraEngine.Tweeners
{
    [AddComponentMenu("HiraTools/Tweeners/Color Tweener")]
    public class ColorTweener : TweenerBase<Color>
    {
        [Space] [Header("Core")]
        [SerializeField] private ColorEvent target = null;
        [SerializeField] private ColorReference startValue = null;
        [SerializeField] private ColorReference endValue = null;
        
        protected override UnityEvent<Color> Target => target;
        protected override ScriptableObjectVariableReference<Color> StartValue => startValue;
        protected override ScriptableObjectVariableReference<Color> EndValue => endValue;
        protected override Func<Color, Color, float, Color> InterpolationFunction => Color.LerpUnclamped;
    }
}