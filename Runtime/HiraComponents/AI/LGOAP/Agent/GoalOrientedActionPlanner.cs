using System;
using System.Collections;
using System.Linq;
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

		public GoalOrientedActionPlanner() => _planRunner = new PlanRunner(OnPlanRunnerFinished);

		[SerializeField] private GameObject targetGameObject = null;
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

		private PlanRunner _planRunner;
        private bool _currentPlanInvalidated = false;
        private byte _actionIndex = byte.MaxValue;

        private void Reset()
		{
			targetGameObject = gameObject;
			blackboard = GetComponent<HiraBlackboardComponent>();
		}

		public void Initialize<T>(ref T initParams)
		{
			_goalResult.First = new PlannerResult(1, Allocator.Persistent) {Count = 1, [0] = byte.MaxValue};
			_goalResult.Second = new PlannerResult(1, Allocator.Persistent) {Count = 1, [0] = byte.MaxValue};

			_plannerDatasets = new RawBlackboardArrayWrapper((byte) (maxPlanLength + 1), blackboard.Template);

			_plannerResult.First = new PlannerResult(maxPlanLength, Allocator.Persistent) {Count = 0};
			_plannerResult.Second = new PlannerResult(maxPlanLength, Allocator.Persistent) {Count = 0};
            
            blackboard.OnKeyEssentialToDecisionMakingUpdate += SchedulePlanner;
            
            SchedulePlanner();
		}

        public void Shutdown()
		{
			_goalResult.First.Dispose();
			_goalResult.Second.Dispose();

			_plannerDatasets.Dispose();

			_plannerResult.First.Dispose();
			_plannerResult.Second.Dispose();

            blackboard.OnKeyEssentialToDecisionMakingUpdate -= SchedulePlanner;
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

				if (_plannerResult.First.CanPop)
				{
					UpdatePlanRunner();
				}
				else
				{
					_currentPlanInvalidated = true;
					unsafe
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

		private void UpdatePlanRunner()
		{
			_actionIndex = _plannerResult.First.Pop();
			var nextAction = domain.Actions[_actionIndex];
			var task = nextAction.Collection4[0].GetExecutable(targetGameObject, blackboard);
			var services = nextAction.Collection5.Select(sp => sp.GetService(targetGameObject, blackboard)).ToArray();
			_planRunner.UpdateTask(task, services);
		}

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
                var plannerResultFirst = _plannerResult.First;
                plannerResultFirst.CurrentIndex = byte.MaxValue;
                _currentPlanInvalidated = false;
            }

			// goal

			var goalResult = _goalResult.Second;
			goalResult.CurrentIndex = _goalResult.First[0];

			var goalCalculator = new GoalCalculatorJob(blackboard.Data, domain.DomainData, goalResult)
				.Schedule();

			_goalResult.Flip();

			// main planner

			var mainPlanner = new MainPlannerJob(
					domain.DomainData[0],
					_goalResult.First,
					_plannerResult.First,
					maxFScore,
					_plannerDatasets, blackboard,
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
                    // the current plan would be invalidated, and so it could never be unchanged
                    
                    // if the planner was scheduled because the plan failed, then it makes no sense that
                    // the plan would remain unchanged
					break;
				case PlannerResultType.Success:
				{
					if (currentResult.CanPop) UpdatePlanRunner();
					else throw new Exception("Unable to pop the plan stack.");
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