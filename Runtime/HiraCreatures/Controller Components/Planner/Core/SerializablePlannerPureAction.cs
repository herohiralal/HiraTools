using System.Collections.Generic;
using System.Linq;

namespace UnityEngine
{
    [CreateAssetMenu(fileName = "New Planner Action", menuName = "Hiralal/HiraEngine/HiraCreatures/Planner Action")]
    public class SerializablePlannerPureAction : SerializablePlannerTransition
    {
        [SerializeField] private SerializableBlackboardModification[] effects = null;
        private IBlackboardModification[] _effects = null;
        public override IReadOnlyList<IBlackboardModification> Effects => _effects;

        protected override void UpdateCache()
        {
            base.UpdateCache();
            var effectsEnumerable = effects.Select(sbm => sbm.Modification);
            _effects = effectsEnumerable is IBlackboardModification[] effectsArray
                ? effectsArray
                : effectsEnumerable.ToArray();
        }
    }
}