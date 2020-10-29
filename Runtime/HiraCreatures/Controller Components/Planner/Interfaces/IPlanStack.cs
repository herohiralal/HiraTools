using UnityEngine;

namespace HiraEngine.Components.Planner
{
    public interface IPlanStack<T> where T : IAction
    {
        T Pop();
        void Consume(T[] actions, int planSize);
        bool HasActions { get; }
        void Invalidate();
    }
}