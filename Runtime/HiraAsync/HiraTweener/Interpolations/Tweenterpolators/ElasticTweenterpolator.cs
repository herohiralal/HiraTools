/*
 * Name: ElasticTweenterpolator.cs
 * Created By: Rohan Jadav
 * Description: Defines easing methods for Elastic Interpolation.
 */

using System;
using UnityEngine;

namespace HiraEngine.Tweeners.Interpolations
{
    internal static class ElasticTweenterpolator
    {
        private static float EaseIn(float state)
        {
            if (state <= 0) return 0;
            if (state >= 1) return 1;

            return -Mathf.Pow(2, (10 * state) - 10) * Mathf.Sin(((state * 10) - 10.75f) * (2 * Mathf.PI / 3));
        }

        private static float EaseOut(float state) => 1 - EaseIn(1 - state);

        private static float EaseInOut(float state)
        {
            if (state <= 0) return 0;
            if (state >= 1) return 1;

            const float c5 = (2 * Mathf.PI) / 4.5f;
            return state < 0.5
                ? -(Mathf.Pow(2, (20 * state) - 10) * Mathf.Sin(((20 * state) - 11.125f) * c5)) / 2
                : 1 + (Mathf.Pow(2, (-20 * state) + 10) * Mathf.Sin(((20 * state) - 11.125f) * c5)) / 2;
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