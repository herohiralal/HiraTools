using System.Collections.Generic;
using System.Linq;
using HiraEngine.Components.Planner;

namespace UnityEngine
{
    [CreateAssetMenu(fileName = "New Planner Hybrid Transition",
        menuName = "Hiralal/HiraEngine/HiraCreatures/Planner Hybrid Transition")]
    public class SerializablePlannerHybridTransition : SerializablePlannerTransition
    {
        [SerializeField] private SerializableBlackboardHybridValue[] targetableEffects = null;
        private IBlackboardHybridValue[] _targetableEffects = null;
        public override IReadOnlyList<IBlackboardQuery> Targets => _targetableEffects;
        public override IReadOnlyList<IBlackboardModification> Effects => _targetableEffects;

        protected override void UpdateCache()
        {
            base.UpdateCache();
            var targetableEffectsEnumerable = targetableEffects.Select(sbh => sbh.HybridValue);
            _targetableEffects = targetableEffectsEnumerable is IBlackboardHybridValue[] targetableEffectsArray
                ? targetableEffectsArray
                : targetableEffectsEnumerable.ToArray();
        }
    }
}