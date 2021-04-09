using HiraEngine.Components.Blackboard.Internal;
using UnityEngine;

namespace HiraEngine.Components.Blackboard
{
    public static class DatabaseFunctionCollectiveExecutionExtensions
    {
        public static bool IsValidOn(this IBlackboardDecorator[] decorators, IBlackboardComponent blackboard)
        {
            foreach (var decorator in decorators)
            {
                if (!decorator.IsValidOn(blackboard))
                    return false;
            }

            return true;
        }

        public static float CalculateScore(this IBlackboardScoreCalculator[] scoreCalculators, IBlackboardComponent blackboard)
        {
            var score = 0f;
            
            foreach (var calculator in scoreCalculators)
                score += calculator.CalculateScore(blackboard);

            return score;
        }

        public static void ApplyTo(this IBlackboardEffector[] effectors, IBlackboardComponent blackboard)
        {
            foreach (var effector in effectors)
                effector.ApplyTo(blackboard);
        }
    }
}