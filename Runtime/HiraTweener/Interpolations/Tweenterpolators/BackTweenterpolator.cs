/*
 * Name: BackTweenterpolator.cs
 * Created By: Rohan Jadav
 * Description: Defines easing methods for Back Interpolation.
 */

using System;
using UnityEngine;

namespace HiraEngine.Tweeners.Interpolations
{
    internal static class BackTweenterpolator
    {
        private static float EaseIn(float state) => (1.70158f * Mathf.Pow(state, 3)) - (2.70158f * Mathf.Pow(state, 2));
        private static float EaseOut(float state) => 1 - EaseIn(1 - state);

        private static float EaseInOut(float state)
        {
            const float c1 = 1.70158f; const float c2 = c1 * 1.525f;

            return 0.5f * (state < 0.5f
                ? Mathf.Pow(2 * state, 2) * (((c2 + 1) * 2 * state) - c2)
                : Mathf.Pow((2 * state) - 2, 2) * (((c2 + 1) * ((state * 2) - 2)) + c2) + 2);
        }

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