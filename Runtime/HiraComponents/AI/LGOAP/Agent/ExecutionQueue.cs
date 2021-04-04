using System;
using System.Collections.Generic;
using System.Diagnostics;
using HiraEngine.Components.AI.Internal;

namespace HiraEngine.Components.AI.LGOAP.Internal
{
	public struct ExecutionQueue
	{
		public static ExecutionQueue Create() => new ExecutionQueue(new Queue<Executable>());
		
		private ExecutionQueue(Queue<Executable> queue)
		{
			_executionQueue = queue;
			_executionQueue.Enqueue(EmptyExecutable.INSTANCE);
			_currentIndex = byte.MaxValue;
			_debugger = null;
		}

		private readonly Queue<Executable> _executionQueue;
		private bool Empty => _executionQueue.Count == 1 && _executionQueue.Peek() == EmptyExecutable.INSTANCE;

		private byte _currentIndex;
		private IPlannerDebugger _debugger;
		public IPlannerDebugger Debugger
		{
			get => _debugger;
			set
			{
				_debugger = value;
				
				UpdateDebuggerExecutableIndex();
			}
		}

		public void Append(Executable executable)
		{
			if (Empty) _executionQueue.Clear();

			_executionQueue.Enqueue(executable);
		}

		public void Start()
		{
			_currentIndex = 0;
			_executionQueue.Peek().OnExecutionStart();
		}

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
						_currentIndex = byte.MaxValue;
						_executionQueue.Enqueue(EmptyExecutable.INSTANCE);
						
						return ExecutionStatus.Succeeded;
					}
					else
					{
						_currentIndex++;
						UpdateDebuggerExecutableIndex();
						_executionQueue.Peek().OnExecutionStart();
						
						return ExecutionStatus.InProgress;
					}
				case ExecutionStatus.Failed:
					_currentIndex = byte.MaxValue;
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
			_currentIndex = byte.MaxValue;
			_executionQueue.Peek().OnExecutionAbort();

			var count = _executionQueue.Count;
			for (var i = 0; i < count; i++) _executionQueue.Dequeue().Dispose();

			_executionQueue.Enqueue(EmptyExecutable.INSTANCE);
		}

		[Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
		private void UpdateDebuggerExecutableIndex()
		{
			try
			{
				Debugger?.UpdateExecutableIndex(_currentIndex);
			}
			catch
			{
				// ignored
			}
		}
	}
}