using System.Collections.Generic;
using System.Linq;

namespace UnityEngine
{
    public abstract class SerializablePlannerTransition : ScriptableObject
    {
#if UNITY_EDITOR
#pragma warning disable CS0414
        // ReSharper disable once NotAccessedField.Local
        [SerializeField] private HiraBlackboardKeySet keySet = null;
#pragma warning restore CS0414
#endif
        [SerializeField] private float baseCost = default;
        [SerializeField] private SerializableBlackboardQuery[] preconditions = null;
        private IBlackboardQuery[] _preconditions = null;

        public virtual void Initialize()
        {
            var preconditionsEnumerable = preconditions.Select(sbq => sbq.Query);
            _preconditions = preconditionsEnumerable is IBlackboardQuery[] preconditionsArray
                ? preconditionsArray
                : preconditionsEnumerable.ToArray();
        }

        public IReadOnlyList<IBlackboardQuery> Preconditions => _preconditions;
        public float BaseCost => baseCost;
        public virtual IReadOnlyList<IBlackboardQuery> Targets { get; } = null;
        public virtual IReadOnlyList<IBlackboardModification> Effects { get; } = null;
    }
}