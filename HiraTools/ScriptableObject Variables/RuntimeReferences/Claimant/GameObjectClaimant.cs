namespace UnityEngine
{
    public class GameObjectClaimant : RuntimeReferenceClaimant<GameObject>
    {
        [SerializeField] private GameObjectReference reference = null;
        protected override RuntimeReference<GameObject> Reference => reference;
    }
}