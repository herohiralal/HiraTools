using System;
using Unity.Burst;
using UnityEngine;

namespace HiraEngine.Components.Blackboard.Internal
{
	[Serializable, BurstCompile]
	public unsafe class IntegerEqualsEffector : ScriptableObject, IBlackboardEffector
	{
		private static FunctionPointer<EffectorDelegate> _effectorFunction;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void CompileFunctions()
		{
			_effectorFunction = BurstCompiler.CompileFunctionPointer<EffectorDelegate>(Effector);
		}

		[HiraCollectionDropdown(typeof(IntegerKey))]
		[SerializeField] public HiraBlackboardKey key = null;
		[SerializeField] public int value = 0;

		public byte MemorySize => sizeof(ushort) + sizeof(int);
		public void AppendMemory(byte* stream) => (*(ushort*) stream, *(int*) (stream + sizeof(ushort))) = (key.Index, value);

		[BurstCompile, AOT.MonoPInvokeCallback(typeof(EffectorDelegate))]
		private static void Effector(byte* blackboard, byte* memory) =>
			*(int*) (blackboard + *(ushort*) memory) = *(int*) (memory + sizeof(ushort));

		public FunctionPointer<EffectorDelegate> Function => _effectorFunction;

		public override string ToString() => key == null ? "INVALID EFFECT" : $"{key.name} equals {value}";
		private void OnValidate() => name = ToString();
	}
}