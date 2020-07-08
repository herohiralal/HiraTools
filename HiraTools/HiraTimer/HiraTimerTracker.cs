using System.Collections;
using Hiralal.CoroutineTracker;

namespace UnityEngine
{
    public interface IHiraTimerTracker : IHiraCoroutineTracker
    {
        float Timer { get; set; }
    }

    internal readonly struct HiraTimerTracker : IHiraTimerTracker
    {
        internal HiraTimerTracker(IEnumerator coroutine, HiraTimerControl control, ulong index) =>
            (this.coroutine, this.control, this.index) = (coroutine, control, index);

        private readonly ulong index;
        private readonly IEnumerator coroutine;
        private readonly HiraTimerControl control;

        //====================================================================== QUERIES

        public bool IsValid => control.IsValid(in index);
        public bool HasStarted => control.HasStarted(in index);
        public bool IsPaused => control.IsPaused(in index);

        //====================================================================== COMMANDS

        public void Start() => control.Start(in index, coroutine);

        public void Resume() => control.Resume(in index);

        public void Pause() => control.Pause(in index);

        public void Stop(bool withOnCompletionCallback = false) => control.Stop(in index, coroutine, withOnCompletionCallback);

        //====================================================================== STATE

        public float Timer
        {
            get => control.GetTimer(in index);
            set => control.SetTimer(in index, in value);
        }

        //====================================================================== EXCEPTION HANDLING
        
        private static void LogInvalidTrackerUsage() => Debug.LogError("Invalid tracker being used.");

        private static void LogAlreadyRunning() => Debug.LogWarning("The requested timer is already running.");

        private static void LogNotRunning() => Debug.LogWarning("The requested timer has not been started yet.");
    }
}