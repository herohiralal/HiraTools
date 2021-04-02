using System;
using System.Collections;
using HiraEngine.Components.AI.LGOAP.Internal;
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
		[SerializeField] private byte[] maxPlanLengths = { };
		[SerializeField] private float[] maxFScores = { };

		private TopLayerRunner _topLayerRunner = null;
		private IntermediateLayerRunner[] _intermediateLayerRunners = null;
		private BottomLayerRunner _bottomLayerRunner = null;
		private TaskRunner _taskRunner = null;

		private bool _isInitialized = false;

		private void OnValidate()
		{
			blackboard = GetComponent<HiraBlackboard>();
			if (domain != null)
			{
				if (domain is IPlannerDomain validPlannerDomain)
				{
					var totalLayerCount = validPlannerDomain.IntermediateLayerCount + 1;

					Array.Resize(ref maxPlanLengths, totalLayerCount);
					for (var i = 0; i < maxPlanLengths.Length; i++)
					{
						maxPlanLengths[i] = (byte) Mathf.Max(maxPlanLengths[i], 2);
					}

					Array.Resize(ref maxFScores, totalLayerCount);
				}
				else
				{
					domain = null;
					Assert.IsTrue(false, "Assigned domain is not a valid planner domain.");
				}
			}
		}

		public void Initialize()
		{
			Assert.IsFalse(_isInitialized);
			Assert.IsTrue(target != null);
			Assert.IsTrue(blackboard != null);
			Assert.IsTrue(domain != null && domain is IPlannerDomain);
			var validDomain = (IPlannerDomain) domain;
			Assert.IsTrue(validDomain.IsInitialized);
			Assert.AreEqual(validDomain.IntermediateLayerCount + 1, maxFScores.Length);
			Assert.AreEqual(validDomain.IntermediateLayerCount + 1, maxPlanLengths.Length);

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
					maxFScores[i]);

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
				maxFScores[i]);

			// task runner
			_bottomLayerRunner.Runner = _taskRunner = new TaskRunner(
				target,
				blackboard,
				validDomain,
				_bottomLayerRunner.OnPlanRunnerFinished);

			_isInitialized = true;
		}

		public void Shutdown() => StartCoroutine(ShutdownCoroutine());

		private IEnumerator ShutdownCoroutine()
		{
			while (_topLayerRunner.SelfOrAnyChildRunning) yield return null;

			_topLayerRunner = null;
			_intermediateLayerRunners = null;
			_bottomLayerRunner = null;

			_isInitialized = false;
		}
	}
}