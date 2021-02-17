using System;
using System.Runtime.CompilerServices;
using UnityEngine.Assertions;

namespace UnityEngine
{
	public class HiraCommandBuffer : MonoBehaviour
	{
		private struct ActiveTimer
		{
			internal bool Active;
			internal float TimeRemaining;
			internal ulong Hash;
			internal Action Action;
		}

		[SerializeField] protected ushort bufferSize = 10;

		private ActiveTimer[] _commandBuffer;
		private System.Collections.Generic.Stack<ushort> _unusedCommandBufferIndices;
		private ulong _hash;

		private void Awake()
		{
			_hash = 1;

			_unusedCommandBufferIndices = new System.Collections.Generic.Stack<ushort>(bufferSize);
			for (ushort i = 0; i < bufferSize; i++) _unusedCommandBufferIndices.Push(i);

			_commandBuffer = new ActiveTimer[bufferSize];
		}

		private void OnDestroy()
		{
			_commandBuffer = null;

			_unusedCommandBufferIndices = null;

			_hash = 1;
		}

		private void Update()
		{
			var count = bufferSize;
			var deltaTime = Time.deltaTime;

			// update all timers first
			for (ushort i = 0; i < count; i++) _commandBuffer[i].TimeRemaining -= _commandBuffer[i].Active ? deltaTime : 0;

			// check which timers are done
			for (ushort i = 0; i < count; i++)
			{
				if (_commandBuffer[i].Active && _commandBuffer[i].TimeRemaining <= 0)
				{
					Invalidate(i);
					_commandBuffer[i].Action.Invoke();
				}
			}
		}

		private void Invalidate(ushort i)
		{
			_commandBuffer[i].Active = false;
			_commandBuffer[i].Hash = 0;
			_unusedCommandBufferIndices.Push(i);
		}

		public TimerHandle SetTimer(Action action, float timer)
		{
			var index = _unusedCommandBufferIndices.Pop();
			var hash = _hash++;

			_commandBuffer[index].Active = true;
			_commandBuffer[index].TimeRemaining = timer;
			_commandBuffer[index].Hash = hash;
			_commandBuffer[index].Action = action;

			return new TimerHandle(this, index, hash);
		}

		internal bool IsHandleValid(in TimerHandle handle) => _commandBuffer[handle.BufferIndex].Hash == handle.Hash;

		internal float GetTimeRemaining(in TimerHandle handle)
		{
			Assert.IsTrue(IsHandleValid(in handle));
			return _commandBuffer[handle.BufferIndex].TimeRemaining;
		}

		internal void SetTimeRemaining(in TimerHandle handle, float value)
		{
			Assert.IsTrue(IsHandleValid(in handle));
			_commandBuffer[handle.BufferIndex].TimeRemaining = value;
		}

		internal void PauseTimer(in TimerHandle handle)
		{
			Assert.IsTrue(IsHandleValid(in handle));
			_commandBuffer[handle.BufferIndex].Active = false;
		}

		internal void ResumeTimer(in TimerHandle handle)
		{
			Assert.IsTrue(IsHandleValid(in handle));
			_commandBuffer[handle.BufferIndex].Active = true;
		}

		internal void CancelTimer(in TimerHandle handle)
		{
			Assert.IsTrue(IsHandleValid(in handle));
			Invalidate(handle.BufferIndex);
		}

		internal void ChangeAction(in TimerHandle handle, Action action)
		{
			Assert.IsTrue(IsHandleValid(in handle));
			_commandBuffer[handle.BufferIndex].Action = action;
		}
	}

	public readonly struct TimerHandle
	{
		public TimerHandle(HiraCommandBuffer owner, ushort bufferIndex, ulong hash)
		{
			_owner = owner;
			BufferIndex = bufferIndex;
			Hash = hash;
		}

		private readonly HiraCommandBuffer _owner;

		internal readonly ushort BufferIndex;
		internal readonly ulong Hash;

		public bool IsValid
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => _owner.IsHandleValid(in this);
		}

		public float TimeRemaining
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => _owner.GetTimeRemaining(in this);
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => _owner.SetTimeRemaining(in this, value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Pause() => _owner.PauseTimer(in this);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Resume() => _owner.ResumeTimer(in this);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Cancel() => _owner.CancelTimer(in this);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ChangeAction(Action action) => _owner.ChangeAction(in this, action);
	}
}