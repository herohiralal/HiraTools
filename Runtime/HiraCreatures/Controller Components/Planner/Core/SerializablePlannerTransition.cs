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

        public virtual void Initialize()
        {
            var preconditionsEnumerable = preconditions.Select(sbq => sbq.Query);
            Preconditions = preconditionsEnumerable is IBlackboardQuery[] preconditionsArray
                ? preconditionsArray
                : preconditionsEnumerable.ToArray();
        }

        public IBlackboardQuery[] Preconditions { get; private set; } = null;
        public float BaseCost => baseCost;
        public virtual IBlackboardQuery[] Targets { get; } = null;
        public virtual IBlackboardModification[] Effects { get; } = null;
    }
}