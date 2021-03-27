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

        public void Initialize<T>(ref T initParams)
        {
            var insistenceCalculators = Goals.Select(g => g.Collection1).ToArray();
            var layer1Targets = Goals.Select(g => g.Collection2).ToArray();
            var layer1Actions =
                Actions.Select<Action, (IBlackboardDecorator[], IBlackboardScoreCalculator[], IBlackboardEffector[])>(a=>
                    (a.Collection1, a.Collection2, a.Collection3)).ToArray();

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