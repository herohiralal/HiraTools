using System;
using HiraEngine.Components.AI.LGOAP.Raw;
using HiraEngine.Components.Blackboard.Internal;
using UnityEngine;

namespace HiraEngine.Components.AI.LGOAP
{
    [CreateAssetMenu(menuName = "Hiralal/AI/LGOAP Domain (1 Intermediate Layer)", fileName = "New LGOAPDomain")]
    [HiraCollectionCustomizer(1, MaxObjectCount = byte.MaxValue, Title = "Goals")]
    [HiraCollectionCustomizer(2, MaxObjectCount = byte.MaxValue, Title = "Intermediate Goals")]
    [HiraCollectionCustomizer(3, MaxObjectCount = byte.MaxValue, Title = "Actions")]
    [HiraCollectionCustomizer(4, MaxObjectCount = byte.MaxValue, 
        RequiredAttributes = new[] {typeof(HiraBlackboardEffectorAttribute)}, Title = "Restarters")]
    public class OneLayeredGoalOrientedActionPlannerDomain : HiraCollection<Goal, IntermediateGoal, Action, IBlackboardEffector>, IInitializable, IPlannerDomain
    {
        [NonSerialized] private RawDomainData _rawDomainData = default;
        public RawDomainData DomainData => _rawDomainData;
        public Goal[] Goals => Collection1;
        public IntermediateGoal[] IntermediateGoals => Collection2;
        public byte IntermediateLayerCount => 1;
        public IntermediateGoal[][] IntermediateLayers => new[] {Collection2};
        public Action[] Actions => Collection3;
        public IBlackboardEffector[] Restarters => Collection4;
        public bool IsInitialized => InitializationStatus == InitializationState.Active;
        public InitializationState InitializationStatus { get; private set; } = InitializationState.Inactive;

        public void Initialize()
        {
            _rawDomainData = RawDomainData.Create(Goals, Actions, IntermediateGoals);
            InitializationStatus = InitializationState.Active;
        }

        public void Shutdown()
        {
            _rawDomainData.Dispose();
            InitializationStatus = InitializationState.Inactive;
        }
    }
}