﻿using System;
using Unity.Burst;
using UnityEngine;

namespace HiraEngine.Components.Blackboard.Internal
{
	[Serializable, BurstCompile]
	public class BooleanEqualsCalculator : BooleanEqualsDecorator, IBlackboardScoreCalculator
	{
		[SerializeField] private float score = 0f;

		public float Score => score;

		public override string ToString() => $"{base.ToString()} ({score:+0;-$#})";
	}
}