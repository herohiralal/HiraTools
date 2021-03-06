﻿namespace HiraEngine.Components.Blackboard.Internal
{
	public interface IBlackboardScoreCalculator : IBlackboardDatabaseFunction<DecoratorDelegate>
	{
		float Score { get; }
        float CalculateScore(UnityEngine.IBlackboardComponent blackboard);
	}
}