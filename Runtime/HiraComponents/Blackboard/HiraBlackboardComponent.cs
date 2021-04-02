namespace UnityEngine
{
    public class HiraBlackboardComponent : HiraBlackboard, IInitializable
    {
        [SerializeField] private HiraBlackboardCore core = null;
        protected override HiraBlackboardCore Core => core;
        public InitializationState InitializationStatus => core.InitializationStatus;
        public void Initialize() => Core.Initialize();
        public void Shutdown() => Core.Shutdown();
    }
}