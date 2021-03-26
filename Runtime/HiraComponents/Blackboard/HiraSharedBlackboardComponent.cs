namespace UnityEngine
{
    public class HiraSharedBlackboardComponent : HiraBlackboard
    {
        [SerializeField] private HiraSharedBlackboard sharedBlackboard = null;
        [SerializeField] private HiraBlackboardCore core = null;
        private void Awake() => core = sharedBlackboard.core;
        private void OnDestroy() => core = null;
        protected override HiraBlackboardCore Core => core;
    }
}