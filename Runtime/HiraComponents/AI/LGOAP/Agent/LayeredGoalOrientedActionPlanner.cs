using System;
using System.Collections;
using HiraEngine.Components.AI.LGOAP.Internal;
using HiraEngine.Components.Blackboard.Raw;
using UnityEngine;
using UnityEngine.Assertions;

namespace HiraEngine.Components.AI.LGOAP
{
	[RequireComponent(typeof(HiraBlackboard))]
	public class LayeredGoalOrientedActionPlanner : MonoBehaviour, IInitializable
	{
		[SerializeField] private HiraComponentContainer target = null;
		[SerializeField] private HiraBlackboard blackboard = null;
		[SerializeField] private ScriptableObject domain = null;
		[SerializeField] private byte maxPlanLength = 4;
		[SerializeField] private float[] maxFScores = { };

		private TopLayerRunner _topLayerRunner = null;
		private IntermediateLayerRunner[] _intermediateLayerRunners = null;
		private BottomLayerRunner _bottomLayerRunner = null;
		private TaskRunner _taskRunner = null;

        private RawBlackboardArrayWrapper _plannerDatasets;

		private void OnValidate()
		{
            maxPlanLength = (byte) Mathf.Max(maxPlanLength, 2);
			blackboard = GetComponent<HiraBlackboard>();
			if (domain != null)
			{
				if (domain is IPlannerDomain validPlannerDomain)
				{
					var totalLayerCount = validPlannerDomain.IntermediateLayerCount + 1;

					Array.Resize(ref maxFScores, totalLayerCount);
				}
				else
				{
					domain = null;
					Assert.IsTrue(false, "Assigned domain is not a valid planner domain.");
				}
			}
		}

        public InitializationState InitializationStatus { get; private set; } = InitializationState.Inactive;

		public void Initialize()
		{
			Assert.AreEqual(InitializationStatus, InitializationState.Inactive);
			Assert.IsTrue(target != null);
			Assert.IsTrue(blackboard != null);
			Assert.IsTrue(domain != null && domain is IPlannerDomain);
			var validDomain = (IPlannerDomain) domain;
			Assert.IsTrue(validDomain.IsInitialized);
			Assert.AreEqual(validDomain.IntermediateLayerCount + 1, maxFScores.Length);
            Assert.IsTrue(maxPlanLength >= 2);

            _plannerDatasets = new RawBlackboardArrayWrapper((byte) (maxPlanLength + 1), blackboard.Template);

			// top layer
			_topLayerRunner = new TopLayerRunner(this, blackboard, validDomain);

			// intermediate layers overall
			IParentLayerRunner currentLink = _topLayerRunner;
			var intermediateLayerCount = validDomain.IntermediateLayerCount;
			_intermediateLayerRunners = new IntermediateLayerRunner[intermediateLayerCount];

			// intermediate layers individual
			byte i;
			for (i = 0; i < intermediateLayerCount; i++)
			{
				var currentIntermediateLayer = _intermediateLayerRunners[i] = new IntermediateLayerRunner(
					currentLink,
					this,
					blackboard,
					validDomain,
					i,
					maxFScores[i],
                    _plannerDatasets,
                    maxPlanLength);

				currentLink.Child = currentIntermediateLayer;
				currentLink = currentIntermediateLayer;
			}

			// bottom layer
			currentLink.Child = _bottomLayerRunner = new BottomLayerRunner(
				currentLink,
				this,
				blackboard,
				validDomain,
				i,
				maxFScores[i],
                _plannerDatasets,
                maxPlanLength);

			// task runner
			_bottomLayerRunner.Runner = _taskRunner = new TaskRunner(
				target,
				blackboard,
				validDomain,
				_bottomLayerRunner.OnPlanRunnerFinished);

            InitializationStatus = InitializationState.Active;

            blackboard.OnKeyEssentialToDecisionMakingUpdate += _topLayerRunner.SchedulePlanner;
		}

		public void Shutdown() => StartCoroutine(ShutdownCoroutine());

		private IEnumerator ShutdownCoroutine()
		{
            Assert.AreEqual(InitializationStatus, InitializationState.Active);
            InitializationStatus = InitializationState.ShuttingDown;
            blackboard.OnKeyEssentialToDecisionMakingUpdate -= _topLayerRunner.SchedulePlanner;
            
            if (_topLayerRunner.SelfOrAnyChildRunning)
            {
                _topLayerRunner.IgnorePlannerResultForSelfAndChild();
                while (_topLayerRunner.SelfOrAnyChildRunning) yield return null;
            }

            if (_topLayerRunner.SelfOrAnyChildScheduled)
                _topLayerRunner.IgnorePlannerResultForSelfAndChild();
            
            _taskRunner.ForceClearTask();
            
            _bottomLayerRunner.Dispose();
            foreach (var runner in _intermediateLayerRunners) runner.Dispose();
            _topLayerRunner.Dispose();
            
            _plannerDatasets.Dispose();

			_topLayerRunner = null;
			_intermediateLayerRunners = null;
			_bottomLayerRunner = null;

            InitializationStatus = InitializationState.Inactive;
		}
	}
}