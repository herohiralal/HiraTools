using HiraEngine.Components.Blackboard.Internal;
using UnityEngine;

namespace HiraEngine.Components.AI.LGOAP
{
    [HiraCollectionCustomizer(1, Title = "Preconditions", MaxObjectCount = byte.MaxValue, RequiredAttributes = new[] {typeof(HiraBlackboardDecoratorAttribute)})]
    [HiraCollectionCustomizer(2, Title = "Cost-Calculators", MaxObjectCount = byte.MaxValue, RequiredAttributes = new[] {typeof(HiraBlackboardScoreCalculatorAttribute)})]
    [HiraCollectionCustomizer(3, Title = "Effects", MaxObjectCount = byte.MaxValue, RequiredAttributes = new[] {typeof(HiraBlackboardEffectorAttribute)})]
    [HiraCollectionCustomizer(4, Title = "Targets", MaxObjectCount = byte.MaxValue, RequiredAttributes = new[] {typeof(IBlackboardDecorator)})]
    public sealed class IntermediateGoal : HiraCollection<IBlackboardDecorator, IBlackboardScoreCalculator, IBlackboardEffector, IBlackboardDecorator>
    {
        public IBlackboardDecorator[] Precondition => Collection1;
        public IBlackboardScoreCalculator[] CostCalculator => Collection2;
        public IBlackboardEffector[] Effect => Collection3;
        public IBlackboardDecorator[] Targets => Collection4;
    }
}