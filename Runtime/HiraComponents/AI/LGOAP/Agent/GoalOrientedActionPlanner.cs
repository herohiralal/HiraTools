using System;
using System.Collections;
using HiraEngine.Components.AI.LGOAP.Internal;
using HiraEngine.Components.Blackboard.Raw;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine;

namespace HiraEngine.Components.AI.LGOAP
{
	[RequireComponent(typeof(HiraBlackboard))]
	public class GoalOrientedActionPlanner : MonoBehaviour, IInitializable
	{
		[Serializable]
		private enum PlannerState
		{
			Idle,
			PlannerScheduled,
			PlannerRunning
		}

		[SerializeField] private HiraComponentContainer targetGameObject = null;
		[SerializeField] private HiraBlackboardComponent blackboard = null;
		[SerializeField] private GoalOrientedActionPlannerDomain domain = null;
		[SerializeField] private byte maxPlanLength = 5;
		[SerializeField] private byte maxFScore = 100;
		[SerializeField] private PlannerState currentState = PlannerState.Idle;

		// goal result
		[NonSerialized] private FlipFlopPool<PlannerResult> _goalResult = default;

		// actions
		[NonSerialized] private RawBlackboardArrayWrapper _plannerDatasets = default;
		[NonSerialized] private FlipFlopPool<PlannerResult> _plannerResult = default;

		private TaskRunner _planRunner;
        private bool _currentPlanInvalidated = false;
        private bool _updateTaskOnUnchangedResult = false;
        private byte _actionIndex = byte.MaxValue;

        public InitializationState InitializationStatus { get; private set; } = InitializationState.Inactive;

        private void Reset()
		{
			blackboard = GetComponent<HiraBlackboardComponent>();
		}

		public void Initialize()
		{
			_goalResult.First = new PlannerResult(1, Allocator.Persistent) {Count = 1, [0] = byte.MaxValue};
			_goalResult.Second = new PlannerResult(1, Allocator.Persistent) {Count = 1, [0] = byte.MaxValue};

			_plannerDatasets = new RawBlackboardArrayWrapper((byte) (maxPlanLength + 1), blackboard.Template);

			_plannerResult.First = new PlannerResult(maxPlanLength, Allocator.Persistent) {Count = 0};
			_plannerResult.Second = new PlannerResult(maxPlanLength, Allocator.Persistent) {Count = 0};
            
            blackboard.OnKeyEssentialToDecisionMakingUpdate += SchedulePlanner;
            
            SchedulePlanner();

            _planRunner = new TaskRunner(targetGameObject, blackboard, domain, OnPlanRunnerFinished);

            InitializationStatus = InitializationState.Active;
		}

        public void Shutdown()
		{
			_goalResult.First.Dispose();
			_goalResult.Second.Dispose();

			_plannerDatasets.Dispose();

			_plannerResult.First.Dispose();
			_plannerResult.Second.Dispose();

            blackboard.OnKeyEssentialToDecisionMakingUpdate -= SchedulePlanner;

            _planRunner = default;

            InitializationStatus = InitializationState.Inactive;
        }

		private void Update() => _planRunner.Update(Time.deltaTime);

		private void OnPlanRunnerFinished(bool success)
		{
			if (success)
			{
				domain.DomainData[0].Break(out _, out var actions);
				actions[_actionIndex].Break(out _, out _, out var effect);
				unsafe
				{
					// this will directly modify the blackboard, and not broadcast any events
					// but that is exactly what we want, because the modification is intended
					effect.Execute((byte*) blackboard.Data.GetUnsafePtr());
				}

				if (_plannerResult.First.CanMoveNext)
				{
                    _plannerResult.First.MoveNext();
					UpdatePlanRunner();
				}
				else
				{
                    // pretend like the planner is executing the first action,
                    // so that MainPlannerJob can check if it can be reused.
                    _plannerResult.First.RestartPlan();

                    _updateTaskOnUnchangedResult = true;

                    if (_goalResult.First[0] == 0) unsafe // if it's the fallback goal
					{
						// this will directly modify the blackboard, and not broadcast any events
						// but that is exactly what we want, because the modification is intended
						domain.DomainData.Restarters.Execute((byte*) blackboard.Data.GetUnsafePtr());
					}

					SchedulePlanner();
				}
			}
			else
            {
                _currentPlanInvalidated = true;
                SchedulePlanner();
            }
		}

        private void SchedulePlanner() => StartCoroutine(RunPlanner());

        private void UpdatePlanRunner() =>
            _planRunner.UpdateTask(_actionIndex = _plannerResult.First.CurrentElement);

		private IEnumerator RunPlanner()
		{
            // exit if a planner is scheduled, will automatically pick up latest data when it runs
			if (currentState == PlannerState.PlannerScheduled) yield break;

			// wait for it to finish its run before it can be run again
			while (currentState == PlannerState.PlannerRunning) yield return null;

			currentState = PlannerState.PlannerScheduled;

			// wait for end of frame to run it
			yield return new WaitForEndOfFrame();

			// update state

			currentState = PlannerState.PlannerRunning;
            
            // invalidate current plan if required
            if (_currentPlanInvalidated)
            {
                _plannerResult.First.InvalidatePlan();
                _currentPlanInvalidated = false;
            }

			// goal

			var goalResult = _goalResult.Second;
			goalResult.CurrentIndex = _goalResult.First[0];

			var goalCalculator = new GoalCalculatorJob(blackboard, domain.DomainData.InsistenceCalculators, goalResult)
				.Schedule();

			_goalResult.Flip();

			// main planner

            _plannerDatasets.CopyFirstFrom(blackboard);

			var mainPlanner = new MainPlannerJob(
					domain.DomainData[0],
					_goalResult.First,
					_plannerResult.First,
					maxFScore,
					_plannerDatasets.Unwrap(),
					_plannerResult.Second)
				.Schedule(goalCalculator);

			_plannerResult.Flip();

			// finish running it before the start of the next frame
			yield return null;

			goalCalculator.Complete();
			mainPlanner.Complete();

			currentState = PlannerState.Idle;

			var currentResult = _plannerResult.First;
			switch (currentResult.ResultType)
			{
				case PlannerResultType.Uninitialized:
					throw new Exception("Planner provided uninitialized output.");
				case PlannerResultType.Failure:
					throw new Exception("Planner failed, possible reason: Max F-Score not high enough, or a parent planner failed.");
				case PlannerResultType.Unchanged:
                    // if the planner was scheduled because of a blackboard update, it means there's
                    // already an active plan and nothing needs to change
                    
                    // if the planner was scheduled because the plan-runner consumed it whole,
                    // the current plan could be reused, and _updateTaskOnUnchangedResult must be used
                    
                    // if the planner was scheduled because the plan failed, then it makes no sense that
                    // the plan would remain unchanged

                    if (_updateTaskOnUnchangedResult)
                    {
                        _updateTaskOnUnchangedResult = false;
                        UpdatePlanRunner();
                    }
                    break;
				case PlannerResultType.Success:
				{
					UpdatePlanRunner();
					break;
				}
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void OnGUI()
		{
			var actions = domain.Actions;
			var current = _plannerResult.First.CurrentIndex - 1;

			for (byte i = 0; i < _plannerResult.First.Count; i++)
			{
				var index = _plannerResult.First[i];
				var actionName = index < actions.Length ? actions[index].name : "INVALID";
				GUILayout.Label($"{i + 1}. {actionName} {(i == current ? "<--" : "")}\n");
			}
		}
	}
}