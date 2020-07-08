using UnityEngine;

namespace Hiralal.CoroutineTracker
{
    public interface IHiraCoroutineTracker
    {
        bool IsValid { get; }
        bool HasStarted { get; }
        bool IsPaused { get; }
        void Start();
        void Resume();
        void Pause();
        void Stop(bool withOnCompletionCallback = false);
    }
}