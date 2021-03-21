using System;
using Unity.Burst;
using UnityEngine;

namespace HiraEngine.Components.Blackboard.Internal
{
	[Serializable, BurstCompile, HiraBlackboardEffector]
	public unsafe class IntegerDynamicAddEffector : ScriptableObject, IBlackboardEffector
	{
		private static FunctionPointer<EffectorDelegate> _effectorFunction;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void CompileFunctions()
		{
			_effectorFunction = BurstCompiler.CompileFunctionPointer<EffectorDelegate>(Effector);
		}

		[HiraCollectionDropdown(typeof(IntegerKey))]
		[SerializeField] public HiraBlackboardKey key = null;
		[HiraCollectionDropdown(typeof(IntegerKey))]
		[SerializeField] public HiraBlackboardKey toAdd = null;

		public byte MemorySize => 2 * sizeof(ushort);

		public void AppendMemory(byte* stream)
		{
			var actualStream = (ushort*) stream;
			actualStream[0] = key.Index;
			actualStream[1] = toAdd.Index;
		}

		[BurstCompile, AOT.MonoPInvokeCallback(typeof(EffectorDelegate))]
		private static void Effector(byte* blackboard, byte* memory)
		{
			var actualStream = (ushort*) memory;
			*(int*) (blackboard + actualStream[0]) += *(int*) (blackboard + actualStream[1]);
		}

		public FunctionPointer<EffectorDelegate> Function => _effectorFunction;

		public override string ToString() => key == null || toAdd == null ? "INVALID EFFECT" : $"{key.name} plus {toAdd.name}";
		private void OnValidate() => name = ToString();
	}
}