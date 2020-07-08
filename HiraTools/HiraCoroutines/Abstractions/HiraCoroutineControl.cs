using System;
using System.Collections;
using UnityEngine;

namespace Hiralal.CoroutineTracker
{
    internal abstract class HiraCoroutineControl
    {
        protected HiraCoroutineControl()
        {
        }

        internal virtual void MarkFree()
        {
            Index = ulong.MinValue;
            IsRunning = false;
        }

        protected ulong Index { get; private protected set; }

        //====================================================================== CORE FUNCTIONALITY

        internal bool IsRunning { get; private protected set; }
        internal Action OnCompletion { get; private protected set; } = null;

        //====================================================================== PAUSING / RESUMING

        internal bool Paused { get; private protected set; } = false;

        //====================================================================== INDEX MATCHING

        protected bool DoesIndexMatch(in ulong index)
        {
            if (index == Index) return true;
            
            LogInvalidTrackerUsage();
            return false;
        }

        private static void LogInvalidTrackerUsage() => Debug.LogError("Invalid tracker being used.");
        
        //====================================================================== INTERFACE

        internal bool IsValid(in ulong index) => index == Index;

        internal bool HasStarted(in ulong index) => DoesIndexMatch(in index) && IsRunning;

        internal bool IsPaused(in ulong index) => !DoesIndexMatch(in index) || Paused;

        internal void Start(in ulong index, IEnumerator coroutine)
        {
            if (!DoesIndexMatch(in index)) return;
            if (IsRunning)
            {
                Debug.LogWarning("The requested coroutine is already running.");
                return;
            }

            IsRunning = true;
            HiraCoroutines.Instance.StartCoroutine(coroutine);
        }

        internal void Stop(in ulong index, IEnumerator coroutine, bool withOnCompletionCallback = false)
        {
            if (!DoesIndexMatch(in index)) return;
            
            if (!IsRunning)
            {
                Debug.LogWarning("The requested coroutine has not been started yet.");
                return;
            }

            HiraCoroutines.Instance.StopCoroutine(coroutine);
            if(withOnCompletionCallback) OnCompletion?.Invoke();
            MarkFree();
        }

        internal void Pause(in ulong index)
        {
            if (DoesIndexMatch(in index)) Paused = true;
        }

        internal void Resume(in ulong index)
        {
            if (DoesIndexMatch(in index)) Paused = false;
        }
    }
}