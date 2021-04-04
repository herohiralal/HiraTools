using System;
using HiraEngine.Components.AI.LGOAP.Raw;
using HiraEngine.Components.Blackboard.Internal;
using UnityEngine;

namespace HiraEngine.Components.AI.LGOAP
{
    [CreateAssetMenu(menuName = "Hiralal/AI/LGOAP Domain (2 Intermediate Layers)", fileName = "New LGOAPDomain")]
    [HiraCollectionCustomizer(1, MaxObjectCount = byte.MaxValue, Title = "Goals")]
    [HiraCollectionCustomizer(2, MaxObjectCount = byte.MaxValue, Title = "Intermediate Goals")]
    [HiraCollectionCustomizer(3, MaxObjectCount = byte.MaxValue, Title = "Intermediate Goals")]
    [HiraCollectionCustomizer(4, MaxObjectCount = byte.MaxValue, Title = "Actions")]
    [HiraCollectionCustomizer(5, MaxObjectCount = byte.MaxValue, 
        RequiredAttributes = new[] {typeof(HiraBlackboardEffectorAttribute)}, Title = "Restarters")]
    public class TwoLayeredGoalOrientedActionPlannerDomain : HiraCollection<Goal, IntermediateGoal, IntermediateGoal, Action, IBlackboardEffector>, IInitializable, IPlannerDomain
    {
        [NonSerialized] private RawDomainData _rawDomainData = default;
        public RawDomainData DomainData => _rawDomainData;
        public Goal[] Goals => Collection1;
        public IntermediateGoal[] IntermediateGoals1 => Collection2;
        public IntermediateGoal[] IntermediateGoals2 => Collection3;
        public byte IntermediateLayerCount => 2;
        public IntermediateGoal[][] IntermediateLayers => new[] {Collection2, Collection3};
        public Action[] Actions => Collection4;
        public IBlackboardEffector[] Restarters => Collection5;
        public bool IsInitialized => InitializationStatus == InitializationState.Active;
        public InitializationState InitializationStatus { get; private set; } = InitializationState.Inactive;

        public void Initialize()
        {
            _rawDomainData = RawDomainData.Create(Goals, Restarters, Actions, IntermediateGoals1, IntermediateGoals2);
            InitializationStatus = InitializationState.Active;
        }

        public void Shutdown()
        {
            _rawDomainData.Dispose();
            InitializationStatus = InitializationState.Inactive;
        }
    }
}