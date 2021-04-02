using System;
using HiraEngine.Components.AI.Internal;
using UnityEngine;

namespace HiraEngine.Components.AI.LGOAP.Internal
{
	public class TaskRunner
	{
		private static readonly Service[] no_services = new Service[0];

		public TaskRunner(HiraComponentContainer target, IBlackboardComponent blackboard, IPlannerDomain domain, Action<bool> onPlanRunnerFinished)
		{
			_domain = domain;
			_target = target;
			_blackboard = blackboard;
			_currentExecutable = EmptyExecutable.INSTANCE;
			_services = no_services;
			_onPlanRunnerFinished = onPlanRunnerFinished;
		}

		private Executable _currentExecutable;
		private Service[] _services;
		private readonly HiraComponentContainer _target;
		private readonly IBlackboardComponent _blackboard;
		private readonly IPlannerDomain _domain;
		private readonly Action<bool> _onPlanRunnerFinished;

		public void UpdateTask(byte index)
		{
			var nextAction = _domain.Actions[index];
			var task = nextAction.GetTask(_target, _blackboard);
			var services = nextAction.GetServices(_target, _blackboard);
			UpdateTask(task, services);
		}

		private void UpdateTask(Executable newExecutable, Service[] services)
		{
			foreach (var service in _services)
				service.OnServiceStop();

			_currentExecutable.OnExecutionAbort();
                    
            foreach (var service in _services)
                service.Dispose();
                    
            _currentExecutable.Dispose();

			_currentExecutable = newExecutable;
			_services = services;

			_currentExecutable.OnExecutionStart();

			foreach (var service in _services)
				service.OnServiceStart();
		}

		public void ForceClearTask() => UpdateTask(EmptyExecutable.INSTANCE, no_services);

		public void Update(float deltaTime)
		{
			var status = _currentExecutable.Execute(deltaTime);

			foreach (var service in _services) service.Run();

			switch (status)
			{
				case ExecutionStatus.Succeeded:
				{
					foreach (var service in _services)
						service.OnServiceStop();

					_currentExecutable.OnExecutionSuccess();
                    
                    foreach (var service in _services)
                        service.Dispose();
                    
                    _currentExecutable.Dispose();

					_currentExecutable = EmptyExecutable.INSTANCE;
					_services = no_services;

					_onPlanRunnerFinished.Invoke(true);

					break;
				}
				case ExecutionStatus.Failed:
				{
					foreach (var service in _services)
						service.OnServiceStop();

					_currentExecutable.OnExecutionFailure();
                    
                    foreach (var service in _services)
                        service.Dispose();
                    
                    _currentExecutable.Dispose();

					_currentExecutable = EmptyExecutable.INSTANCE;
					_services = no_services;

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