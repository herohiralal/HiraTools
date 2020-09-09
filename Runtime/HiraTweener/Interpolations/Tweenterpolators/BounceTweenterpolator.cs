﻿/*
 * Name: BounceTweenterpolator.cs
 * Created By: Rohan Jadav
 * Description: Defines easing methods for Bounce Interpolation.
 */

using System;
using HiraTweener.Interpolations.Core;

namespace HiraTweener.Interpolations.Tweenterpolators
{
    internal static class BounceTweenterpolator
    {
        private static float EaseOut(float state)
        {
            const float n1 = 7.5625f;
            const float d1 = 2.75f;

            if (state < 1 / d1) return n1 * state * state;

            if (state < 2 / d1) return n1 * (state -= 1.5f / d1) * state + 0.75f;

            if (state < 2.5 / d1) return n1 * (state -= 2.25f / d1) * state + 0.9375f;

            return n1 * (state -= 2.625f / d1) * state + 0.984375f;
        }

        private static float EaseIn(float state) => 1 - EaseOut(1 - state);

        private static float EaseInOut(float state) => 0.5f * (state < 0.5f
            ? EaseIn(2 * state)
            : 1 + EaseOut((2 * state) - 1));

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