﻿using System.Runtime.CompilerServices;
using UnityEngine;

namespace HiraEngine.Components.AI.Internal
{
    public class WaitExecutable : Executable
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public WaitExecutable Init(float time)
        {
            _time = time;
            return this;
        }

        private float _time;

        public override ExecutionStatus Execute(float deltaTime)
        {
            _time -= deltaTime;
            return _time <= 0f ? ExecutionStatus.Succeeded : ExecutionStatus.InProgress;
        }

        public override void Dispose() => GenericPool<WaitExecutable>.Return(this);
    }
}