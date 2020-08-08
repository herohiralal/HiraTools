using Hiralal.HiraWorlds;
using UnityEngine.SceneManagement;

namespace UnityEngine
{
    [System.Serializable]
    public class HiraWorldLoader
    {
        [SerializeField] private HiraWorld world = null;
        [SerializeField] private bool loadAsynchronously = true;
        [SerializeField] private LocalPhysicsMode localPhysicsMode = LocalPhysicsMode.None;
        [SerializeField] private bool setActiveOnLoad = false;
        [SerializeField] private bool unloadAllEmbeddedSceneObjects = false;

        public int BuildIndex => world.BuildIndex;
        public bool IsLoaded => world.IsLoaded;

        public void SubmitRequestForWorldToLoad() => 
            world.SubmitLoadingRequest(loadAsynchronously, localPhysicsMode, setActiveOnLoad);

        public void WithdrawRequestForWorldToLoad() => 
            world.WithdrawLoadingRequest(unloadAllEmbeddedSceneObjects);
    }
}