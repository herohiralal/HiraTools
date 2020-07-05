﻿﻿using System;
using UnityEngine;

namespace HiraPoolTool.Abstractions
{
    public interface IPool : IDisposable
    {
        int PoolCount { get; }
        void AddToPool(byte count);
        void RemoveFromPool(byte count);
        void AddToPool();
        void RemoveFromPool();
        Component GetObject();
        void ReturnObject(Component component);
    }
}