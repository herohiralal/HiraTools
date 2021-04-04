using System;
using UnityEngine;

namespace HiraEngine.Components.AI.LGOAP.Internal
{
	public class TaskRunner
	{
		public TaskRunner(HiraComponentContainer target, IBlackboardComponent blackboard, IPlannerDomain domain, Action<bool> onPlanRunnerFinished)
		{
			_domain = domain;
			_target = target;
			_blackboard = blackboard;
			_executionQueue = ExecutionQueue.Create();
			_serviceRunner = ServiceRunner.Create();
			_onPlanRunnerFinished = onPlanRunnerFinished;
		}

		private readonly ExecutionQueue _executionQueue;
		private readonly ServiceRunner _serviceRunner;
		private readonly HiraComponentContainer _target;
		private readonly IBlackboardComponent _blackboard;
		private readonly IPlannerDomain _domain;
		private readonly Action<bool> _onPlanRunnerFinished;

		public void UpdateTask(byte index)
		{
			ForceClearTask();

			_domain.Actions[index].Populate(_executionQueue, _serviceRunner, _target, _blackboard);

			_executionQueue.Start();
			_serviceRunner.Start();
		}

		public void ForceClearTask()
		{
			_serviceRunner.Stop();
			_executionQueue.Stop();
		}

		public void Update(float deltaTime)
		{
			var status = _executionQueue.Process(deltaTime);

			_serviceRunner.Process();

			switch (status)
			{
				case ExecutionStatus.Succeeded:
				{
					_serviceRunner.Stop();
					_onPlanRunnerFinished.Invoke(true);
					break;
				}
				case ExecutionStatus.Failed:
				{
					_serviceRunner.Stop();
					_onPlanRunnerFinished.Invoke(false);
					break;
				}
				case ExecutionStatus.InProgress:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}