/*
 * Name: LinearTweenterpolator.cs
 * Created By: Rohan Jadav
 * Description: Defines easing methods for Linear Interpolation.
 */

using System;
using UnityEngine;

namespace HiraEngine.Tweeners.Interpolations
{
    internal static class LinearTweenterpolator
    {
        private static float Interpolate(float state) => state;

        internal static Func<float, float> GetInterpolationMethod(HiraTweenEaseType easeType) => Interpolate;
    }
}