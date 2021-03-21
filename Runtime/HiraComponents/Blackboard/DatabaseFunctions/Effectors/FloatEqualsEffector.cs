using System;
using Unity.Burst;
using UnityEngine;

namespace HiraEngine.Components.Blackboard.Internal
{
	[Serializable, BurstCompile, HiraBlackboardEffector]
	public unsafe class FloatEqualsEffector : ScriptableObject, IBlackboardEffector
	{
		private static FunctionPointer<EffectorDelegate> _effectorFunction;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void CompileFunctions()
		{
			_effectorFunction = BurstCompiler.CompileFunctionPointer<EffectorDelegate>(Effector);
		}

		[HiraCollectionDropdown(typeof(FloatKey))]
		[SerializeField] public HiraBlackboardKey key = null;
		[SerializeField] public float value = 0;

		public byte MemorySize => sizeof(ushort) + sizeof(float);
		public void AppendMemory(byte* stream) => (*(ushort*) stream, *(float*) (stream + sizeof(ushort))) = (key.Index, value);

		[BurstCompile, AOT.MonoPInvokeCallback(typeof(EffectorDelegate))]
		private static void Effector(byte* blackboard, byte* memory) =>
			*(float*) (blackboard + *(ushort*) memory) = *(float*) (memory + sizeof(ushort));

		public FunctionPointer<EffectorDelegate> Function => _effectorFunction;

		public override string ToString() => key == null ? "INVALID EFFECT" : $"{key.name} equals {value}";
		private void OnValidate() => name = ToString();
	}
}