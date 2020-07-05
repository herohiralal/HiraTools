﻿/*
 * Name: SineTweenterpolator.cs
 * Created By: Rohan Jadav
 * Description: Defines easing methods for Sinuous Interpolation.
 */

using System;
using UnityEngine;
using HiraTweener.Interpolations.Core;

namespace HiraTweener.Interpolations.Tweenterpolators
{
    internal static class SineTweenterpolator
    {
        private static float EaseIn(float state) => 1 - Mathf.Cos(0.5f * state * Mathf.PI);
        private static float EaseOut(float state) => Mathf.Sin(0.5f * state * Mathf.PI);
        private static float EaseInOut(float state) => (1 - Mathf.Cos(Mathf.PI * state)) / 2;

        internal static Func<float, float> GetInterpolationMethod(HiraTweenEaseType easeType)
        {
            // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
            switch (easeType)
            {
                case HiraTweenEaseType.EaseIn: return EaseIn;
                case HiraTweenEaseType.EaseOut: return EaseOut;
                case HiraTweenEaseType.EaseInOut: return EaseInOut;
                default: throw new ArgumentOutOfRangeException(nameof(easeType), easeType, null);
            }
        }
    }
}