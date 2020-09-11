using System;
using System.Collections.Generic;
using System.Linq;
using Hiralal.Blackboard;
using Hiralal.GOAP.Transitions;
using UnityEngine;

// ReSharper disable PossibleMultipleEnumeration

namespace Hiralal.GOAP.Planner
{
    public class Planner<T> where T : IHiraWorldStateTransition
    {
        public Planner(HiraBlackboard blackboard, Action<Stack<T>> planSetter)
        {
            _blackboard = blackboard;
            _state = blackboard.GetDuplicateWorldState();
            _planSetter = planSetter;
        }

        private readonly HiraBlackboard _blackboard;
        private readonly HiraBlackboardValueSet _state;
        private readonly Action<Stack<T>> _planSetter;
        
        private IReadOnlyList<HiraBlackboardValue> _target = null;
        private IEnumerable<T> _actions = null;

        private float _maxFScore = 0;
        private Stack<T> _plan = null;

        public void Initialize(float newMaxFScore, HiraWorldStateTransition goal, IEnumerable<T> actions)
        {
            _plan = new Stack<T>();
            _maxFScore = newMaxFScore;
            HiraBlackboardValueSet.Copy(_blackboard.ValueSet, _state);
            _target = goal.Effects;
            _actions = actions;
        }

        public void GeneratePlan(object context = null)
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