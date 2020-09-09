﻿/*
 * Name: CircularTweenterpolator.cs
 * Created By: Rohan Jadav
 * Description: Defines easing methods for Circular Interpolation.
 */

using System;
using UnityEngine;
using HiraTweener.Interpolations.Core;

namespace HiraTweener.Interpolations.Tweenterpolators
{
    internal static class CircularTweenterpolator
    {
        private static float EaseOut(float state) => Mathf.Sqrt(1 - Mathf.Pow(state - 1, 2));
        private static float EaseIn(float state) => 1 - EaseOut(1 - state);

        private static float EaseInOut(float state) =>
            0.5f * (state < 0.5f ? EaseIn(-2 * state) : 1 + EaseOut(-2 * state));

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