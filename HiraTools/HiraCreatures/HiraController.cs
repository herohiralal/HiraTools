namespace UnityEngine
{
    [AddComponentMenu("HiraTools/HiraCreatures/HiraController")]
    public class HiraController : MonoBehaviour
    {
        [SerializeField] private HiraCreature controlledCreature = null;
        public HiraCreature ControlledCreature => controlledCreature;

        public virtual void Possess(HiraCreature creature)
        {
            controlledCreature = creature;
        }

        public virtual void Unpossess()
        {
            controlledCreature = null;
        }
    }
}