using System.Collections;
using System.Collections.Generic;

namespace UnityEngine
{
    [AddComponentMenu("HiraTools/HiraCreatures/HiraController")]
    public abstract class HiraController : MonoBehaviour, IEnumerable<IHiraControllerComponent>
    {
        [SerializeField] private HiraCreature controlledCreature = null;
        public HiraCreature ControlledCreature => controlledCreature;

        public abstract void Initialize<T>(in T spawnParameters);

        public abstract void Stop();

        public virtual void Possess(HiraCreature creature)
        {
            controlledCreature = creature;
        }

        public virtual void Dispossess()
        {
            controlledCreature = null;
        }

        public abstract IEnumerator<IHiraControllerComponent> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}