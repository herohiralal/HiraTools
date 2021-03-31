using System.Linq;
using HiraEngine.Components.AI.Internal;
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

        public Executable GetTask(HiraComponentContainer target, IBlackboardComponent blackboard) =>
            Collection4.Length switch
            {
                0 => AutoFailExecutable.INSTANCE,
                1 => Collection4[0].GetExecutable(target, blackboard),
                _ => GenericPool<SequenceExecutable>.Retrieve().Init(Collection4, target, blackboard)
            };

        public Service[] GetServices(HiraComponentContainer target, IBlackboardComponent blackboard) =>
            Collection5.Select(sp => sp.GetService(target, blackboard)).ToArray();
    }
}