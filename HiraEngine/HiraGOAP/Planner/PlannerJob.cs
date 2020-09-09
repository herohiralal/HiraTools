using System;
using System.Collections.Generic;
using System.Linq;
using Hiralal.Blackboard;

// ReSharper disable PossibleMultipleEnumeration

namespace Hiralal.GOAP.Planner
{
    public readonly struct PlannerJob<T> where T : IHiraWorldStateTransition
    {
        public PlannerJob(float maxFScore, HiraBlackboardValueSet state, IReadOnlyList<HiraBlackboardValue> target,
            IEnumerable<T> actions, Action<Stack<T>> planSetter)
        {
            this.maxFScore = maxFScore;
            this.state = state;
            this.target = target;
            this.actions = actions;
            plan = new Stack<T>();
            this.planSetter = planSetter;
        }

        private readonly float maxFScore;
        private readonly HiraBlackboardValueSet state;
        private readonly IReadOnlyList<HiraBlackboardValue> target;
        private readonly IEnumerable<T> actions;
        private readonly Stack<T> plan;
        private readonly Action<Stack<T>> planSetter;

        public void GeneratePlan()
        {
            float threshold = Heuristic;
            while (true)
            {
                var score = PerformHeuristicEstimatedSearch(0, threshold);
                if (!score.HasValue)
                {
                    planSetter(plan);
                    return;
                }
                if (score.Value > maxFScore)
                {
                    planSetter(null);
                    return;
                }
                threshold = score.Value;
            }
        }

        private float? PerformHeuristicEstimatedSearch(float cost, float threshold)
        {
            // Heuristic is the number of values still remaining
            var heuristic = Heuristic;

            // Check if the estimated cost is larger than threshold
            var fScore = cost + heuristic;
            if (fScore > threshold) return fScore;

            // Goal reached
            if (heuristic == 0) return null;

            var min = float.MaxValue;
            foreach (var action in actions)
            {
                // ignore invalid actions
                if (!action.ArePreConditionsSatisfied(state)) continue;

                // Mutate the state
                var undoBuffer = state.ApplyAction(action);

                // Calculate new cost
                var actionCost = action.Cost;
                cost += actionCost;

                // check if the goal state was reached
                var score = PerformHeuristicEstimatedSearch(cost, threshold);

                // Undo cost mutation
                cost -= actionCost;

                // Undo state mutation
                state.Undo(undoBuffer);

                // If the goal state was reached
                if (!score.HasValue)
                {
                    plan.Push(action);
                    return null;
                }

                // If a goal state wasn't reached & the new fScore is smaller than before
                if (score.Value < min) min = score.Value;
            }

            return min;
        }

        private int Heuristic => target.Count(state.DoesNotContainValue);
    }
}