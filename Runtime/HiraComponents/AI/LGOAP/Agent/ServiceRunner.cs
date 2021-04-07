using System.Collections.Generic;

namespace HiraEngine.Components.AI.LGOAP.Internal
{
	public readonly struct ServiceRunner
	{
		public static ServiceRunner Create() => new ServiceRunner(new List<Service>());
		
		private ServiceRunner(List<Service> services) => _services = services;

		private readonly List<Service> _services;

		public void Append(Service service) => _services.Add(service);

		public void Start()
		{
			foreach (var service in _services) service.OnServiceStart();
		}

		public void Process()
		{
			foreach (var service in _services) service.Run();
		}

		public void Stop()
		{
			foreach (var service in _services) service.OnServiceStop();

			foreach (var service in _services) service.Dispose();
			
			_services.Clear();
		}
	}
}