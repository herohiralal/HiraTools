using System;
using System.Linq;
using HiraEngine.Components.AI.LGOAP.Raw;
using HiraEngine.Components.Blackboard.Internal;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace HiraEngine.Components.AI.LGOAP
{
    [CreateAssetMenu(menuName = "Hiralal/AI/GOAP Domain", fileName = "New GOAPDomain")]
    [HiraCollectionCustomizer(1, MaxObjectCount = byte.MaxValue, Title = "Goals")]
    [HiraCollectionCustomizer(2, MaxObjectCount = byte.MaxValue, Title = "Actions")]
    [HiraCollectionCustomizer(3, MaxObjectCount = byte.MaxValue, 
	    RequiredAttributes = new[] {typeof(HiraBlackboardEffectorAttribute)}, Title = "Restarters")]
    public unsafe class GoalOrientedActionPlannerDomain : HiraCollection<Goal, Action, IBlackboardEffector>, IInitializable
    {
        [NonSerialized] private NativeArray<byte> _domainData = default;
        [NonSerialized] private RawDomainData _rawDomainData = default;
        public RawDomainData DomainData => _rawDomainData;
        public Goal[] Goals => Collection1;
        public Action[] Actions => Collection2;
        public IBlackboardEffector[] Restarters => Collection3;

        public void Initialize()
        {
            var insistenceCalculators = Goals.Select(g => g.InsistenceCalculators).ToArray();
            var layer1Targets = Goals.Select(g => g.Targets).ToArray();
            var layer1Actions =
                Actions.Select<Action, (IBlackboardDecorator[], IBlackboardScoreCalculator[], IBlackboardEffector[])>(a=>
                    (a.Precondition, a.CostCalculator, a.Effect)).ToArray();

            var size = RawDomainData.GetSize(insistenceCalculators, Restarters, (layer1Targets, layer1Actions));

            _domainData = new NativeArray<byte>(size, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            _rawDomainData = RawDomainData.Create(insistenceCalculators, Restarters, (byte*) _domainData.GetUnsafePtr(), (layer1Targets, layer1Actions));
        }

        public void Shutdown()
        {
            _domainData.Dispose();
            _rawDomainData = new RawDomainData();
        }
    }
}