namespace UnityEngine
{
    [AddComponentMenu("HiraTools/Runtime References/Transform Claimant")]
    public class TransformReferenceClaimant : RuntimeReferenceClaimant<Transform>
    {
        [SerializeField] private TransformReference reference = null;
        protected override RuntimeReference<Transform> Reference => reference;
    }
}