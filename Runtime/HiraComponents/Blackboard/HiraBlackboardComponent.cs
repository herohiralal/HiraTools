namespace UnityEngine
{
    public class HiraBlackboardComponent : HiraBlackboard, IInitializable
    {
        [SerializeField] private HiraBlackboardCore core = null;
        protected override HiraBlackboardCore Core => core;
        public void Initialize<T>(ref T initParams) => Core.Initialize(ref initParams);
        public void Shutdown() => Core.Shutdown();
    }
}