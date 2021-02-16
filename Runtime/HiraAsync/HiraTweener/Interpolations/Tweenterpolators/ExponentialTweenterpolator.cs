/*
 * Name: ExponentialTweenterpolator.cs
 * Created By: Rohan Jadav
 * Description: Defines easing methods for Exponential Interpolation.
 */

using System;
using UnityEngine;

namespace HiraEngine.Tweeners.Interpolations
{
    internal static class ExponentialTweenterpolator
    {
        private static float EaseOut(float state) => state >= 1 ? 1 : 1 - Mathf.Pow(2, -10 * state);
        private static float EaseIn(float state) => 1 - EaseOut(1 - state);

        private static float EaseInOut(float state) =>
            0.5f * (state < 0.5f ? EaseIn(2 * state) : EaseOut(2 * state));

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