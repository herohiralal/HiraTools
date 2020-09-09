using System;
using System.Collections.Generic;
using System.Linq;
using Hiralal.Blackboard;
using Hiralal.GOAP.Transitions;
using UnityEngine;

// ReSharper disable PossibleMultipleEnumeration

namespace Hiralal.GOAP.Planner
{
    public readonly struct PlannerJob<T> where T : IHiraWorldStateTransition
    {
        public PlannerJob(float maxFScore, HiraBlackboard blackboard, IReadOnlyList<HiraBlackboardValue> target,
            IEnumerable<T> actions, Action<Stack<T>> planSetter)
        {
            this._maxFScore = maxFScore;
            _state = blackboard.GetDuplicateWorldState();
            this._target = target;
            this._actions = actions;
            _plan = new Stack<T>();
            this._planSetter = planSetter;
        }

        private readonly float _maxFScore;
        private readonly HiraBlackboardValueSet _state;
        private readonly IReadOnlyList<HiraBlackboardValue> _target;
        private readonly IEnumerable<T> _actions;
        private readonly Stack<T> _plan;
        private readonly Action<Stack<T>> _planSetter;

        public void GeneratePlan()
        {
            float threshold = Heuristic;
            while (true)
            {
                var score = PerformHeuristicEstimatedSearch(0, threshold);
                if (!score.HasValue)
                {
                    _planSetter(_plan);
                    return;
                }
                if (score.Value > _maxFScore)
                {
                    _planSetter(null);
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
            foreach (var action in _actions)
            {
                // ignore invalid actions
                if (!action.ArePreConditionsSatisfied(_state)) continue;

                // Mutate the state
                var undoBuffer = _state.ApplyAction(action);

                // Calculate new cost
                var actionCost = action.Cost;
                cost += actionCost;

                // check if the goal state was reached
                var score = PerformHeuristicEstimatedSearch(cost, threshold);

                // Undo cost mutation
                cost -= actionCost;

                // Undo state mutation
                _state.Undo(undoBuffer);

                // If the goal state was reached
                if (!score.HasValue)
                {
                    _plan.Push(action);
                    return null;
                }

                // If a goal state wasn't reached & the new fScore is smaller than before
                if (score.Value < min) min = score.Value;
            }

            return min;
        }

        private int Heuristic => _target.Count(_state.DoesNotContainValue);
    }
}