using System;
using HiraEngine.Components.AI.LGOAP.Raw;
using HiraEngine.Components.Blackboard.Internal;
using UnityEngine;

namespace HiraEngine.Components.AI.LGOAP
{
    [CreateAssetMenu(menuName = "Hiralal/AI/GOAP Domain (0 Intermediate Layers)", fileName = "New GOAPDomain")]
    [HiraCollectionCustomizer(1, MaxObjectCount = byte.MaxValue, Title = "Goals")]
    [HiraCollectionCustomizer(2, MaxObjectCount = byte.MaxValue, Title = "Actions")]
    [HiraCollectionCustomizer(3, MaxObjectCount = byte.MaxValue, 
	    RequiredAttributes = new[] {typeof(HiraBlackboardEffectorAttribute)}, Title = "Restarters")]
    public class GoalOrientedActionPlannerDomain : HiraCollection<Goal, Action, IBlackboardEffector>, IInitializable, IPlannerDomain
    {
        [NonSerialized] private RawDomainData _rawDomainData = default;
        public RawDomainData DomainData => _rawDomainData;
        public Goal[] Goals => Collection1;
        public byte IntermediateLayerCount => 0;
        public IntermediateGoal[][] IntermediateLayers { get; } = { };
        public Action[] Actions => Collection2;
        public IBlackboardEffector[] Restarters => Collection3;
        public bool IsInitialized => InitializationStatus == InitializationState.Active;
        public InitializationState InitializationStatus { get; private set; } = InitializationState.Inactive;

        public void Initialize()
        {
            _rawDomainData = RawDomainData.Create(Goals, Actions);
            InitializationStatus = InitializationState.Active;
        }

        public void Shutdown()
        {
            _rawDomainData.Dispose();
            InitializationStatus = InitializationState.Inactive;
        }
    }
}