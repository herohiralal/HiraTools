using System;

namespace HiraEngine.Components.AI.LGOAP.Internal
{
	public delegate void OnPlanRunnerFinished(bool success);

	public struct PlanRunner
	{
		private static readonly IService[] no_services = new IService[0];

		public PlanRunner(OnPlanRunnerFinished onPlanRunnerFinished)
		{
			_currentExecutable = EmptyExecutable.INSTANCE;
			_services = no_services;
			_onPlanRunnerFinished = onPlanRunnerFinished;
		}

		private IExecutable _currentExecutable;
		private IService[] _services;
		private readonly OnPlanRunnerFinished _onPlanRunnerFinished;

		public void UpdateTask(IExecutable newExecutable, IService[] services = null)
		{
			services ??= no_services;

			foreach (var service in _services)
				service.OnServiceStop();

			_currentExecutable.OnExecutionAbort();

			_currentExecutable = newExecutable;
			_services = services;

			_currentExecutable.OnExecutionStart();

			foreach (var service in _services)
				service.OnServiceStart();
		}

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