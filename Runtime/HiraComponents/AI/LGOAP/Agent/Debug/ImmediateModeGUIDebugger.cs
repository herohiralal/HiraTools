using UnityEngine;
using Object = UnityEngine.Object;

namespace HiraEngine.Components.AI.LGOAP.Internal
{
	[RequireComponent(typeof(LayeredGoalOrientedActionPlanner))]
	public class ImmediateModeGUIDebugger : MonoBehaviour, IPlannerDebugger
	{
		private LayeredGoalOrientedActionPlanner _planner = null;
		private IPlannerDomain _domain = null;
		
		private void Awake()
		{
			_planner = GetComponent<LayeredGoalOrientedActionPlanner>();
			if (_planner.InitializationStatus != InitializationState.Active)
			{
				Debug.LogError("Planner not initialized.");
				Destroy(this);
				return;
			}

			_domain = _planner.Domain;
			_intermediateGoals = new string[_domain.IntermediateLayerCount][];
			_activeGoalsIndices = new byte[_domain.IntermediateLayerCount];
			for (var i = 0; i < _activeGoalsIndices.Length; i++) _activeGoalsIndices[i] = byte.MaxValue;

			_planner.Debugger = this;
		}

		private void OnDestroy()
		{
			_domain = null;
			_planner = null;
		}

		private string _goalName = "";
		private string[][] _intermediateGoals = null;
		private byte[] _activeGoalsIndices = null;
		private string[] _actions = { };
		private byte _activeActionIndex = byte.MaxValue;
		private string[][] _executables;
		private byte _currentExecutionIndex = byte.MaxValue;
		private string[][] _services;

		public void UpdateGoal(byte index)
		{
			var goals = _domain.Goals;
			if (goals.Length <= index) return;

			var goal = goals[index];
			if (goal == null) return;

			_goalName = goal.name;
		}

		public void UpdateIntermediatePlan(byte layerIndex, PlannerResult plan)
		{
			if (layerIndex >= _domain.IntermediateLayerCount) return;
			
			var planCount = plan.Count;
			var planBufferSize = plan.BufferSize;
			if (planCount > planBufferSize) return;

			var actionPool = _domain.IntermediateLayers[layerIndex];
			var actionPoolLength = actionPool.Length;
			var currentPlan = _intermediateGoals[layerIndex] = new string[planCount];
			
			for (byte i = 0; i < planCount; i++)
			{
				if (plan[i] < actionPoolLength && actionPool[i] != null)
					currentPlan[i] = actionPool[i].name;
				else
					currentPlan[i] = "INVALID";
			}

			_activeGoalsIndices[layerIndex] = plan.CurrentIndex;
		}

		public void IncrementIntermediateGoalIndex(byte layerIndex)
		{
			if (layerIndex >= _domain.IntermediateLayerCount) return;

			_activeGoalsIndices[layerIndex]++;
		}

		public void UpdateCorePlan(PlannerResult plan)
		{
			var planCount = plan.Count;
			var planBufferSize = plan.BufferSize;
			if (planCount > planBufferSize) return;

			var actionPool = _domain.Actions;
			var actionPoolLength = actionPool.Length;
			var currentPlan = _actions = new string[planCount];

			_executables = new string[planCount][];
			_services = new string[planCount][];

			for (byte i = 0; i < planCount; i++)
			{
				var currentAction = actionPool[i];
				if (plan[i] < actionPoolLength && currentAction != null)
				{
					currentPlan[i] = currentAction.name;

					var executablesPool = currentAction.Collection4;
					var executablesCount = executablesPool.Length;
					var currentExecutables = _executables[i] = new string[executablesCount];
					for (var j = 0; j < executablesCount; j++)
					{
						currentExecutables[j] = ((Object) executablesPool[j]).name;
					}

					var servicesPool = currentAction.Collection5;
					var servicesCount = servicesPool.Length;
					var currentServices = _services[i] = new string[servicesCount];
					for (var j = 0; j < servicesCount; j++)
					{
						currentServices[j] = ((Object) servicesPool[j]).name;
					}
				}
				else
				{
					currentPlan[i] = "INVALID";

					_executables[i] = new string[0];
					_services[i] = new string[0];
				}
			}

			_activeActionIndex = plan.CurrentIndex;
		}

		public void IncrementActionIndex()
		{
			_activeActionIndex++;
		}

		public void UpdateExecutableIndex(byte newIndex)
		{
			_currentExecutionIndex = newIndex;
		}

		private void OnGUI()
		{
			using (new GUILayout.HorizontalScope())
			{
				using (new GUILayout.VerticalScope())
				{
					GUILayout.Label(">> Goals <<");
					GUILayout.Label(_goalName);
				}

				for (var i = 0; i < _intermediateGoals.Length; i++)
				{
					var intermediatePlan = _intermediateGoals[i];
					var current = _activeGoalsIndices[i];
					using (new GUILayout.VerticalScope())
					{
						GUILayout.Label($">> Layer {i} <<");
						for (var j = 0; j < intermediatePlan.Length; j++)
						{
							var s = intermediatePlan[j];
							GUILayout.Label(j == current ? $"[{s}]" : s);
						}
					}
				}

				using (new GUILayout.VerticalScope())
				{
					GUILayout.Label(">> Tasks <<");
					for (var i = 0; i < _actions.Length; i++)
					{
						using (new GUILayout.HorizontalScope())
						{
							var s = _actions[i];
							GUILayout.Label(i == _activeActionIndex ? $"[{s}]" : s);
							
							var currentExecutables = _executables[i];
							for (var j = 0; j < currentExecutables.Length; j++)
							{
								var s2 = currentExecutables[j];
								GUILayout.Label(i == _activeActionIndex && j == _currentExecutionIndex ? $"[{s2}]" : s2);
							}

							var currentServices = _services[i];
							foreach (var s2 in currentServices)
							{
								GUILayout.Label(i == _activeActionIndex ? $"({s2})" : s2);
							}
						}
					}
				}
			}
		}
	}
}