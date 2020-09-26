using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine
{
    public abstract class SerializablePlannerTransition : ScriptableObject
    {
        [SerializeField] private HiraBlackboardKeySet keySet = null;
        [SerializeField] private SerializableBlackboardQuery[] preconditions = null;
        private IBlackboardQuery[] _preconditions = null;

        private void Awake()
        {
            if (keySet == null) return;
            UpdateCache();
        }

        private void OnValidate()
        {
            try
            {
                UpdateCache();
            }
            catch (NullReferenceException)
            {
                // Ignore
            }
        }

        protected virtual void UpdateCache()
        {
            var preconditionsEnumerable = preconditions.Select(sbq => sbq.Query);
            _preconditions = preconditionsEnumerable is IBlackboardQuery[] preconditionsArray
                ? preconditionsArray
                : preconditionsEnumerable.ToArray();
        }

        public IReadOnlyList<IBlackboardQuery> Preconditions => _preconditions;
        public virtual IReadOnlyList<IBlackboardQuery> Targets { get; } = null;
        public virtual IReadOnlyList<IBlackboardModification> Effects { get; } = null;
    }
}