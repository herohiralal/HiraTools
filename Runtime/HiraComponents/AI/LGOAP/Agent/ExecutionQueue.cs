using System;
using System.Collections.Generic;
using HiraEngine.Components.AI.Internal;

namespace HiraEngine.Components.AI.LGOAP.Internal
{
	public readonly struct ExecutionQueue
	{
		public static ExecutionQueue Create() => new ExecutionQueue(new Queue<Executable>());
		
		private ExecutionQueue(Queue<Executable> queue)
		{
			_executionQueue = queue;
			_executionQueue.Enqueue(EmptyExecutable.INSTANCE);
		}

		private readonly Queue<Executable> _executionQueue;
		private bool Empty => _executionQueue.Count == 1 && _executionQueue.Peek() == EmptyExecutable.INSTANCE;

		public void Append(Executable executable)
		{
			if (Empty) _executionQueue.Clear();

			_executionQueue.Enqueue(executable);
		}

		public void Start() => _executionQueue.Peek().OnExecutionStart();

		public ExecutionStatus Process(float deltaTime)
		{
			var status = _executionQueue.Peek().Execute(deltaTime);
			switch (status)
			{
				case ExecutionStatus.InProgress:
					return ExecutionStatus.InProgress;
				case ExecutionStatus.Succeeded:
					_executionQueue.Peek().OnExecutionSuccess();
					_executionQueue.Dequeue().Dispose();
					if (_executionQueue.Count == 0)
					{
						_executionQueue.Enqueue(EmptyExecutable.INSTANCE);
						
						return ExecutionStatus.Succeeded;
					}
					else
					{
						_executionQueue.Peek().OnExecutionStart();
						
						return ExecutionStatus.InProgress;
					}
				case ExecutionStatus.Failed:
					_executionQueue.Peek().OnExecutionFailure();

					var count = _executionQueue.Count;
					for (var i = 0; i < count; i++) _executionQueue.Dequeue().Dispose();

					_executionQueue.Enqueue(EmptyExecutable.INSTANCE);
					
					return ExecutionStatus.Failed;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public void Stop()
		{
			_executionQueue.Peek().OnExecutionAbort();

			var count = _executionQueue.Count;
			for (var i = 0; i < count; i++) _executionQueue.Dequeue().Dispose();

			_executionQueue.Enqueue(EmptyExecutable.INSTANCE);
		}
	}
}