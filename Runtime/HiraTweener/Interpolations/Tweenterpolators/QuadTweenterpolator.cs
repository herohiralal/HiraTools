﻿/*
 * Name: QuadTweenterpolator.cs
 * Created By: Rohan Jadav
 * Description: Defines easing methods for Quad Interpolation.
 */

using System;
using UnityEngine;
using HiraTweener.Interpolations.Core;

namespace HiraTweener.Interpolations.Tweenterpolators
{
    internal static class QuadTweenterpolator
    {
        private static float EaseIn(float state) => Mathf.Pow(state, 2);
        private static float EaseOut(float state) => 1 - EaseIn(1 - state);
        private static float EaseInOut(float state) => state < 0.5f ? 2 * EaseIn(state) : 1 - 2 * EaseIn(1 - state);

        internal static Func<float, float> GetInterpolationMethod(HiraTweenEaseType easeType) =>
            easeType switch
            {
                HiraTweenEaseType.EaseIn => EaseIn,
                HiraTweenEaseType.EaseOut => EaseOut,
                HiraTweenEaseType.EaseInOut => EaseInOut,
                _ => throw new ArgumentOutOfRangeException(nameof(easeType), easeType, null)
            };
    }
}