using HiraEngine.Components.Blackboard.Internal;
using Unity.Burst;
using UnityEngine;

namespace HiraEngine.Components.AI.LGOAP
{
    [BurstCompile]
    [HiraCollectionCustomizer(1, Title = "Insistence-Calculators", MaxObjectCount = byte.MaxValue, RequiredAttributes = new[] {typeof(HiraBlackboardScoreCalculatorAttribute)})]
    [HiraCollectionCustomizer(2, Title = "Targets", MaxObjectCount = byte.MaxValue, RequiredAttributes = new[] {typeof(HiraBlackboardDecoratorAttribute)})]
    public sealed class Goal : HiraCollection<IBlackboardScoreCalculator, IBlackboardDecorator>
    {
    }
}