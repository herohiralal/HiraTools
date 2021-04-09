using System;
using Unity.Burst;
using UnityEngine;

namespace HiraEngine.Components.Blackboard.Internal
{
	[Serializable, BurstCompile, HiraBlackboardEffector]
	public unsafe class BooleanFlipEffector : ScriptableObject, IBlackboardEffector
	{
		private static FunctionPointer<EffectorDelegate> _effectorFunction;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void CompileFunctions()
		{
			_effectorFunction = BurstCompiler.CompileFunctionPointer<EffectorDelegate>(Effector);
		}

		[HiraCollectionDropdown(typeof(BooleanKey))]
		[SerializeField] public HiraBlackboardKey key = null;

		public byte MemorySize => sizeof(ushort);
		public void AppendMemory(byte* stream) => *(ushort*) stream = key.Index;

		[BurstCompile, AOT.MonoPInvokeCallback(typeof(EffectorDelegate))]
		private static void Effector(byte* blackboard, byte* memory) =>
			blackboard[*(ushort*) memory] = (byte) (1 - (blackboard[*(ushort*) memory]));

		public FunctionPointer<EffectorDelegate> Function => _effectorFunction;

        public void ApplyTo(IBlackboardComponent blackboard) => blackboard.SetValue<byte>(key.Index, (byte) (1 - blackboard.GetValue<byte>(key.Index)));

		public override string ToString() => key == null ? "INVALID EFFECT" : $"Flip {key.name}";
		private void OnValidate() => name = ToString();
	}
}