using System.Threading;

namespace UnityEngine
{
    public interface IHiraControllerPlanner<T> : IHiraControllerComponent, IPlanner<T> where T : IAction
    {
    }

    public interface IPlanner<T> where T : IAction
    {
        bool IsActive { get; }
        IPlanner<T> Initialize();
        IPlanner<T> ForGoal(IBlackboardQuery[] goal);
        IPlanner<T> WithAvailableTransitions(T[] actions);
        IPlanner<T> WithMaxFScore(float maxFScore);
        IPlanner<T> WithCancellationToken(CancellationToken ct);
        void WithCallback(PlannerCompletionCallback<T> completionCallback);
        void Run();
        void RunMultiThreaded();
    }

    public delegate void PlannerCompletionCallback<in T>(PlannerResult result, T[] plan, int planLength) where T : IAction;

    public enum PlannerResult
    {
        None, Success, Failure, Cancelled
    }

    public interface IAction
    {
        string Name { get; }
        IBlackboardQuery[] Preconditions { get; }
        IBlackboardModification[] Effects { get; }
        void BuildPrePlanCache();
        float Cost { get; }
    }
    public interface IBlackboardQuery
    {
        bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet);
    }
    
    public interface IBlackboardModification
    {
        void ApplyTo(IReadWriteBlackboardDataSet dataSet);
        void ApplyTo(IBlackboardValueAccessor valueAccessor);
    }
}