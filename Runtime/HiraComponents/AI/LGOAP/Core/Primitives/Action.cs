using HiraEngine.Components.AI.Internal;
using HiraEngine.Components.AI.LGOAP.Internal;
using HiraEngine.Components.Blackboard.Internal;
using UnityEngine;

namespace HiraEngine.Components.AI.LGOAP
{
    [HiraCollectionCustomizer(1, Title = "Precondition", MaxObjectCount = byte.MaxValue, RequiredAttributes = new[] {typeof(HiraBlackboardDecoratorAttribute)})]
    [HiraCollectionCustomizer(2, Title = "Cost-Calculator", MaxObjectCount = byte.MaxValue, RequiredAttributes = new[] {typeof(HiraBlackboardScoreCalculatorAttribute)})]
    [HiraCollectionCustomizer(3, Title = "Effect", MaxObjectCount = byte.MaxValue, RequiredAttributes = new[] {typeof(HiraBlackboardEffectorAttribute)})]
    [HiraCollectionCustomizer(4, Title = "Tasks", MaxObjectCount = byte.MaxValue, MinObjectCount = 1)]
    [HiraCollectionCustomizer(5, Title = "Services", MaxObjectCount = byte.MaxValue)]
    public sealed class Action : HiraCollection<IBlackboardDecorator, IBlackboardScoreCalculator, IBlackboardEffector, IExecutableProvider, IServiceProvider>
    {
        public IBlackboardDecorator[] Precondition => Collection1;
        public IBlackboardScoreCalculator[] CostCalculator => Collection2;
        public IBlackboardEffector[] Effect => Collection3;

        public void Populate(ExecutionQueue executionQueue, ServiceRunner serviceRunner, HiraComponentContainer target, IBlackboardComponent blackboard)
        {
	        if (Collection4.Length == 0)
		        executionQueue.Append(AutoSucceedExecutable.INSTANCE);

	        foreach (var provider in Collection4)
		        executionQueue.Append(provider.GetExecutable(target, blackboard));

	        foreach (var serviceProvider in Collection5)
		        serviceRunner.Append(serviceProvider.GetService(target, blackboard));
        }
    }
}