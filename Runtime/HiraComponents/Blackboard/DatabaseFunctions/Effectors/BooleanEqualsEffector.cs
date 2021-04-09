using System;
using Unity.Burst;
using UnityEngine;

namespace HiraEngine.Components.Blackboard.Internal
{
	[Serializable, BurstCompile, HiraBlackboardEffector]
	public unsafe class BooleanEqualsEffector : ScriptableObject, IBlackboardEffector
	{
		private static FunctionPointer<EffectorDelegate> _equalsTrueEffectorFunction;
		private static FunctionPointer<EffectorDelegate> _equalsFalseEffectorFunction;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void CompileFunctions()
		{
			_equalsTrueEffectorFunction = BurstCompiler.CompileFunctionPointer<EffectorDelegate>(EqualsTrueEffector);
			_equalsFalseEffectorFunction = BurstCompiler.CompileFunctionPointer<EffectorDelegate>(EqualsFalseEffector);
		}

		[HiraCollectionDropdown(typeof(BooleanKey))]
		[SerializeField] public HiraBlackboardKey key = null;
		[SerializeField] public bool value = true;

		public byte MemorySize => sizeof(ushort);
		public void AppendMemory(byte* stream) => *(ushort*) stream = key.Index;

		[BurstCompile, AOT.MonoPInvokeCallback(typeof(EffectorDelegate))]
		private static void EqualsTrueEffector(byte* blackboard, byte* memory) => blackboard[*(ushort*) memory] = 1;

		[BurstCompile, AOT.MonoPInvokeCallback(typeof(EffectorDelegate))]
		private static void EqualsFalseEffector(byte* blackboard, byte* memory) => blackboard[*(ushort*) memory] = 0;

		public FunctionPointer<EffectorDelegate> Function =>
			value
				? _equalsTrueEffectorFunction
				: _equalsFalseEffectorFunction;

        public void ApplyTo(IBlackboardComponent blackboard) => blackboard.SetValue<byte>(key.Index, value.ToByte());

		public override string ToString() => key == null ? "INVALID EFFECT" : $"{key.name} equals {value}";
		private void OnValidate() => name = ToString();
	}
}