using System;
using System.Collections;
using HiraEngine.Components.AI.LGOAP.Internal;
using HiraEngine.Components.Blackboard.Raw;
using UnityEngine;

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

        public HiraComponentContainer Target
        {
	        get => target;
	        set
	        {
		        if (InitializationStatus != InitializationState.Inactive)
			        throw new Exception("Cannot change the target of the planner while it's running.");

		        if (value == null)
			        throw new ArgumentNullException(nameof(Target));

		        target = value;
	        }
        }

        public HiraBlackboard Blackboard => blackboard;

        public IPlannerDomain Domain
        {
	        get => (IPlannerDomain) domain;
	        set
	        {
		        if (value == null)
			        throw new ArgumentNullException(nameof(Domain));
		        
		        if (!(value is ScriptableObject scriptableObject))
			        throw new Exception("The planner domain must be a ScriptableObject.");

		        domain = scriptableObject;
	        }
        }

        public byte MaxPlanLength
        {
	        get => maxPlanLength;
	        set
	        {
		        if (value <= 2)
			        throw new InvalidOperationException("A plan with 1 or 0 actions is called an action.");
		        
		        maxPlanLength = value;
	        }
        }

        public float[] MaxFScores
        {
	        get => maxFScores;
	        set => maxFScores = value;
        }

        private void Update() => _taskRunner.Update(Time.deltaTime);

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
					throw new InvalidOperationException("Assigned domain is not a valid planner domain.");
				}
			}
		}

        public InitializationState InitializationStatus { get; private set; } = InitializationState.Inactive;

		public void Initialize()
		{
			if (InitializationStatus != InitializationState.Inactive)
				throw new InvalidOperationException($"Initialize() called on planner {gameObject.name} even though it's already initialized.");

			if (target == null)
				throw new NullReferenceException($"Target not found on planner {gameObject.name}.");

			if (blackboard == null)
				throw new NullReferenceException($"Planner {gameObject.name} needs a blackboard to initialize.");

			if (domain == null || !(domain is IPlannerDomain {IsInitialized: true} validDomain))
				throw new NullReferenceException($"Planner {gameObject.name} does not have a valid domain.");

			if (validDomain.IntermediateLayerCount + 1 != maxFScores.Length)
				throw new Exception($"Planner {gameObject.name} does not have correct number of max F-Scores");

			if (maxPlanLength < 2)
				throw new Exception($"Planner {gameObject.name} has max plan length set to less than 2, which is not called a plan.");

            _plannerDatasets = new RawBlackboardArrayWrapper((byte) (maxPlanLength + 1), blackboard.Template);

			// top layer
			_topLayerRunner = new TopLayerRunner(this, blackboard, validDomain, _plannerDatasets);

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
            
            _topLayerRunner.SchedulePlanner();
		}

		public void Shutdown() => StartCoroutine(ShutdownCoroutine());

		private IEnumerator ShutdownCoroutine()
		{
			if (InitializationStatus != InitializationState.Active)
				throw new InvalidOperationException($"Shutdown() called on {gameObject.name} even though its not initialized.");
			
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