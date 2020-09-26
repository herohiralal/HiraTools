using System.Collections.Generic;
using System.Linq;

namespace UnityEngine
{
    [CreateAssetMenu(fileName = "New Planner Goal", menuName = "Hiralal/HiraEngine/HiraCreatures/Planner Goal")]
    public class SerializablePlannerPureGoal : SerializablePlannerTransition
    {
        [SerializeField] private SerializableBlackboardQuery[] target = null;
        private IBlackboardQuery[] _targets = null;
        public override IReadOnlyList<IBlackboardQuery> Targets => _targets;

        protected override void UpdateCache()
        {
            base.UpdateCache();
            var targetsEnumerable = target.Select(sbq => sbq.Query);
            _targets = targetsEnumerable is IBlackboardQuery[] targetsArray
                ? targetsArray
                : targetsEnumerable.ToArray();
        }
    }
}