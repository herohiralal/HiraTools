using HiraEngine.Components.Blackboard.Internal;
using UnityEngine;

namespace HiraEngine.Components.AI.LGOAP
{
    [HiraCollectionCustomizer(1, Title = "Preconditions", MaxObjectCount = byte.MaxValue, RequiredAttributes = new[] {typeof(HiraBlackboardDecoratorAttribute)})]
    [HiraCollectionCustomizer(2, Title = "Cost-Calculators", MaxObjectCount = byte.MaxValue, RequiredAttributes = new[] {typeof(HiraBlackboardScoreCalculatorAttribute)})]
    [HiraCollectionCustomizer(3, Title = "Effects", MaxObjectCount = byte.MaxValue, RequiredAttributes = new[] {typeof(HiraBlackboardEffectorAttribute)})]
    [HiraCollectionCustomizer(4, Title = "Task", MaxObjectCount = 1)]
    [HiraCollectionCustomizer(5, Title = "Services")]
    public sealed class Action : HiraCollection<IBlackboardDecorator, IBlackboardScoreCalculator, IBlackboardEffector, IExecutableProvider, IServiceProvider>
    {
    }
}