using System;
using Hiralal.CoroutineTracker;

namespace UnityEngine
{
    public interface IHiraTimerTracker
    {
        float Timer { get; set; }
        bool IsPaused { get; }
        void Resume();
        void Pause();
        void Stop();
    }

    internal readonly struct HiraTimerTracker : IHiraTimerTracker
    {
        internal HiraTimerTracker(Coroutine coroutine, HiraTimerControl control) =>
            (this.coroutine, this.control) = (coroutine, control);

        private readonly Coroutine coroutine;
        private readonly HiraTimerControl control;
        public float Timer { get => control.Timer; set => control.Timer = value; }
        public bool IsPaused => control.IsPaused;
        public void Resume() => control.IsPaused = false;
        public void Pause() => control.IsPaused = true;
        public void Stop()
        {
            HiraCoroutines.Instance.StopCoroutine(coroutine);
            control.MarkFree();
        }
    }
}