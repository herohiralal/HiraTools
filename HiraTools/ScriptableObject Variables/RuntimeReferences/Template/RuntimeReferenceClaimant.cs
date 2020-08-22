namespace UnityEngine
{
    public abstract class RuntimeReferenceClaimant<T> : MonoBehaviour where T : Object
    {
        [SerializeField] private T target = null;

        protected abstract RuntimeReference<T> Reference { get; }

        private void Awake()
        {
            if (Reference.Value == null) Reference.Claim(target);
        }

        private void OnDestroy()
        {
            if (Reference.Value == target) Reference.Release();
        }
    }
}