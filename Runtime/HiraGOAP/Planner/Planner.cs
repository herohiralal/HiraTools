using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hiralal.Blackboard;
using Hiralal.GOAP.Transitions;
using UnityEngine;

namespace Hiralal.GOAP.Planner
{
    public class Planner<T> where T : IHiraWorldStateTransition
    {
        public Planner(HiraBlackboard blackboard, Action<Stack<T>> planSetter)
        {
            _blackboard = blackboard;
            _state = blackboard.KeySet.GetFreshValueSet();
            _planSetter = planSetter;
        }

        private readonly HiraBlackboard _blackboard;
        private readonly HiraBlackboardValueSet _state;
        private readonly Action<Stack<T>> _planSetter;
        
        private IReadOnlyList<HiraBlackboardValue> _target = null;
        private IEnumerable<T> _actions = null;

        private CancellationToken _ct;
        private float _maxFScore = 0;
        private int _maxIterationsPerFrame = 0;
        private int _iterationsThisFrame = 1;
        private Stack<T> _plan = null;

        public void Initialize(float newMaxFScore, HiraWorldStateTransition goal, IEnumerable<T> actions, int maxIterationsPerFrame, CancellationToken ct)
        {
            _plan = new Stack<T>();
            _iterationsThisFrame = 1;
            _maxFScore = newMaxFScore;
            _maxIterationsPerFrame = maxIterationsPerFrame;
            HiraBlackboardValueSet.Copy(_blackboard.ValueSet, _state);
            _target = goal.Effects;
            _actions = actions.ToArray();
            _ct = ct;
        }

        public async void GeneratePlan(object context = null)
        {
            float threshold = Heuristic;
            while (true)
            {
                var score = await PerformHeuristicEstimatedSearch(0, threshold);
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

                if (score.Value < 0)
                    return;

                threshold = score.Value;
            }
        }

        private async Task<float?> PerformHeuristicEstimatedSearch(float cost, float threshold)
        {
            if (_ct.IsCancellationRequested) return -1;
            
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
                var score = await PerformHeuristicEstimatedSearch(cost, threshold);

                // Undo cost mutation
                cost -= actionCost;

                // Undo state mutation
                _state.Undo(undoBuffer);

                await AsynchronousCheck();

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

        private async Task AsynchronousCheck()
        {
            _iterationsThisFrame++;
            if (_iterationsThisFrame > _maxIterationsPerFrame)
            {
                _iterationsThisFrame = 1;
                await Task.Yield();
            }
        }

        private int Heuristic => _target.Count(_state.DoesNotContainValue);
    }
}