using System;
using Unity.Burst;
using UnityEngine;

namespace HiraEngine.Components.Blackboard.Internal
{
	[Serializable, BurstCompile, HiraBlackboardScoreCalculator]
	public class VectorAlmostEqualsCalculator : VectorAlmostEqualsDecorator, IBlackboardScoreCalculator
	{
		[SerializeField] private float score = 0f;

		public float Score => score;
        public override byte MemorySize => (byte) (sizeof(float) + base.MemorySize);

        public override unsafe void AppendMemory(byte* stream)
        {
            *(float*) stream = Score;
            base.AppendMemory(stream + sizeof(float));
        }

        public float CalculateScore(IBlackboardComponent blackboard) => IsValidOn(blackboard) ? score : 0f;

		public override string ToString() => $"{base.ToString()} ({score:+0;-#})";
	}
}