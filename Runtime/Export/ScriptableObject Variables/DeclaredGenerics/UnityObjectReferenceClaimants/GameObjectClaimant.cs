namespace UnityEngine
{
    [AddComponentMenu("HiraTools/Runtime References/GameObject Claimant")]
    public class GameObjectClaimant : RuntimeReferenceClaimant<GameObject>
    {
        [SerializeField] private GameObjectReference reference = null;
        protected override RuntimeReference<GameObject> Reference => reference;
    }
}