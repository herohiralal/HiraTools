using System;
using System.Collections;
using System.Linq;
using HiraEngine.Components.AI.LGOAP.Internal;
using HiraEngine.Components.Blackboard.Raw;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace HiraEngine.Components.AI.LGOAP
{
    [RequireComponent(typeof(HiraBlackboardComponent))]
    public class GoalOrientedActionPlanner : MonoBehaviour
    {
        public GoalOrientedActionPlanner() => _planRunner = new PlanRunner(OnPlanRunnerFinished);

        [SerializeField] private GameObject targetGameObject = null;
        [SerializeField] private HiraBlackboardComponent blackboard = null;
        [SerializeField] private GoalOrientedActionPlannerDomain domain = null;
        [SerializeField] private byte maxPlanLength = 5;
        [SerializeField] private byte maxFScore = 100;

        // goal result
        [NonSerialized] private FlipFlopPool<PlannerResult> _goalResult = default;

        // actions
        [NonSerialized] private RawBlackboardArrayWrapper _plannerDatasets = default;
        [NonSerialized] private FlipFlopPool<PlannerResult> _plannerResult = default;

        private PlanRunner _planRunner;

        private void Reset()
        {
            targetGameObject = gameObject;
            blackboard = GetComponent<HiraBlackboardComponent>();
        }

        private void Awake()
        {
            _goalResult.First = new PlannerResult(1, Allocator.Persistent) {Count = 1, [0] = byte.MaxValue};
            _goalResult.Second = new PlannerResult(1, Allocator.Persistent) {Count = 1, [0] = byte.MaxValue};

            _plannerDatasets = new RawBlackboardArrayWrapper((byte) (maxPlanLength + 1), blackboard.Template);

            _plannerResult.First = new PlannerResult(maxPlanLength, Allocator.Persistent) {Count = 0};
            _plannerResult.Second = new PlannerResult(maxPlanLength, Allocator.Persistent) {Count = 0};
        }

        private void OnDestroy()
        {
            _goalResult.First.Dispose();
            _goalResult.Second.Dispose();

            _plannerDatasets.Dispose();

            _plannerResult.First.Dispose();
            _plannerResult.Second.Dispose();
        }

        public void OnPlanRunnerFinished(bool success)
        {
            if (success && _plannerResult.First.CanPop) // action was successful
            {
                var nextActionIndex = _plannerResult.First.Pop();
                var nextAction = domain.Collection2[nextActionIndex];
                var task = nextAction.Collection4[0].GetExecutable(targetGameObject, blackboard);
                var services = nextAction.Collection5.Select(sp => sp.GetService(targetGameObject, blackboard)).ToArray();
                _planRunner.UpdateTask(task, services);
            }
            else
            {
                // todo: schedule the planner
            }
        }

        private IEnumerator SchedulePlanner()
        {
            yield return new WaitForEndOfFrame();
            
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
                    maxFScore,
                    _plannerDatasets, blackboard,
                    _plannerResult.Second)
                .Schedule(goalCalculator);

            _plannerResult.Flip();

            yield return null;
            
            goalCalculator.Complete();
            
            // todo: process goal calculator output
            
            mainPlanner.Complete();
            
            // todo: process main planner output
        }
    }
}