using System.Collections.Generic;
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
        IPlanner<T> ForGoal(IEnumerable<IBlackboardQuery> goal);
        IPlanner<T> WithAvailableTransitions(IEnumerable<T> actions);
        IPlanner<T> WithMaxFScore(float maxFScore);
        IPlanner<T> WithCancellationToken(CancellationToken ct);
        void WithCallback(PlannerCompletionCallback<T> completionCallback);
        void GeneratePlan(object obj = null);
    }

    public delegate void PlannerCompletionCallback<in T>(PlannerResult result, T[] plan) where T : IAction;

    public enum PlannerResult
    {
        None, Success, FScoreOverflow, Cancelled
    }

    public interface IAction
    {
        IReadOnlyList<IBlackboardQuery> Preconditions { get; }
        IReadOnlyList<IBlackboardModification> Effects { get; }
        void BuildPrePlanCache();
        float Cost { get; }
    }
    public interface IBlackboardQuery
    {
        bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet);
    }
    
    public interface IBlackboardModification
    {
        IBlackboardModification ApplyTo(IReadWriteBlackboardDataSet dataSet);
    }
}