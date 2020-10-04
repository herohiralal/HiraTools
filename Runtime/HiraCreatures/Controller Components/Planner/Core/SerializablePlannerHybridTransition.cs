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
        // ReSharper disable once CoVariantArrayConversion
        public override IBlackboardQuery[] Targets => _targetableEffects;
        // ReSharper disable once CoVariantArrayConversion
        public override IBlackboardModification[] Effects => _targetableEffects;

        public override void Initialize()
        {
            base.Initialize();
            var targetableEffectsEnumerable = targetableEffects.Select(sbh => sbh.HybridValue);
            _targetableEffects = targetableEffectsEnumerable is IBlackboardHybridValue[] targetableEffectsArray
                ? targetableEffectsArray
                : targetableEffectsEnumerable.ToArray();
        }
    }
}