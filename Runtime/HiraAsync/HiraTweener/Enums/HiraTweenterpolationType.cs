﻿/*
 * Name: HiraTweenterpolationType.cs
 * Created By: Rohan Jadav
 * Description: The exhaustive values of interpolation types.
 */

using System;

namespace UnityEngine
{
    [Serializable]
    public enum HiraTweenterpolationType
    {
        Linear,
        Quad,
        Cubic,
        Quart,
        Quint,
        Sine,
        Exponential,
        Circular,
        Back,
        Elastic,
        Bounce
    }
}