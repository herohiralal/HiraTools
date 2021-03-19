using System;
using Unity.Burst;
using UnityEngine;

namespace HiraEngine.Components.Blackboard.Internal
{
	[Serializable, BurstCompile]
	public unsafe class VectorEqualsEffector : ScriptableObject, IBlackboardEffector
	{
		private static FunctionPointer<EffectorDelegate> _effectorFunction;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void CompileFunctions()
		{
			_effectorFunction = BurstCompiler.CompileFunctionPointer<EffectorDelegate>(Effector);
		}

		[HiraCollectionDropdown(typeof(VectorKey))]
		[SerializeField] public HiraBlackboardKey key = null;
		[SerializeField] public Vector3 value = Vector3.zero;

		public byte MemorySize => sizeof(ushort) + (sizeof(float) * 3);

		public void AppendMemory(byte* stream)
		{
			var keyPtr = (ushort*) stream;
			var xPtr = (float*) (stream + sizeof(ushort));
			var yPtr = xPtr + 1;
			var zPtr = yPtr + 1;

			*keyPtr = key.Index;
			*xPtr = value.x;
			*yPtr = value.y;
			*zPtr = value.z;
		}

		[BurstCompile, AOT.MonoPInvokeCallback(typeof(EffectorDelegate))]
		private static void Effector(byte* blackboard, byte* memory)
		{
			var keyPtr = (ushort*) memory;
			var xValuePtr = (float*) (memory + sizeof(ushort));
			var xKeyPtr = (float*) (blackboard + *keyPtr);

			for (var i = 0; i < 3; i++) xKeyPtr[i] = xValuePtr[i];
		}

		public FunctionPointer<EffectorDelegate> Function => _effectorFunction;

		public override string ToString() => key == null ? "INVALID EFFECT" : $"{key.name} equals {value}";
		private void OnValidate() => name = ToString();
	}
}