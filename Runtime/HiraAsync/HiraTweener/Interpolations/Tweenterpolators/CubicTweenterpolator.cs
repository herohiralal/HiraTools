/*
 * Name: CubicTweenterpolator.cs
 * Created By: Rohan Jadav
 * Description: Defines easing methods for Cubic Interpolation.
 */

using System;
using UnityEngine;

namespace HiraEngine.Tweeners.Interpolations
{
    internal static class CubicTweenterpolator
    {
        private static float EaseIn(float state) => Mathf.Pow(state, 3);
        private static float EaseOut(float state) => 1 - EaseIn(1 - state);

        private static float EaseInOut(float state) =>
            (state < 0.5f ? 4 * EaseIn(state) : 1 - 4 * EaseIn(1 - state));

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