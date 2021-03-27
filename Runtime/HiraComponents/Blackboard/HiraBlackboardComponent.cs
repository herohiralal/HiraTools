namespace UnityEngine
{
    public class HiraBlackboardComponent : HiraBlackboard, IInitializable
    {
        [SerializeField] private HiraBlackboardCore core = null;
        protected override HiraBlackboardCore Core => core;
        public void Initialize() => Core.Initialize();
        public void Shutdown() => Core.Shutdown();
    }
}