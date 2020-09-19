using System.Collections;
using HiraEngine.CoroutineTracker;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HiraEngine.HiraWorlds
{
    [CreateAssetMenu(fileName = "New HiraWorld", menuName = "Hiralal/HiraWorld")]
    public class HiraWorld : ScriptableObject
    {
        [SerializeField] private int buildIndex = default;
        [SerializeField] public bool isLoaded = false;

        public int BuildIndex => buildIndex;
        public bool IsLoaded => Application.isPlaying && SceneManager.GetSceneByBuildIndex(buildIndex).isLoaded;

        private void OnEnable() => _loadRequests = 0;
        private void OnDisable() => _loadRequests = 0;

        private uint _loadRequests = 0;

        public void SubmitLoadingRequest(bool asynchronous, LocalPhysicsMode localPhysicsMode, bool setActive)
        {
            _loadRequests++;
            if (_loadRequests > 1) return;

            var loadSceneParameters = new LoadSceneParameters(LoadSceneMode.Additive, localPhysicsMode);
            if (asynchronous)
            {
                var operation = SceneManager.LoadSceneAsync(buildIndex, loadSceneParameters);
            
                operation.completed += o =>
                {
                    if (!o.isDone) return;
                    var scene = SceneManager.GetSceneByBuildIndex(buildIndex);
                    if (setActive) SceneManager.SetActiveScene(scene);
                };
            }
            else
            {
                var scene = SceneManager.LoadScene(buildIndex, loadSceneParameters);
                if (setActive) HiraCoroutines.Instance.StartCoroutine(SetActiveScene(scene));
            }
        }

        public void WithdrawLoadingRequest(bool unloadAllEmbeddedSceneObjects)
        {
            _loadRequests--;
            if (_loadRequests > 0) return;

            SceneManager.UnloadSceneAsync(buildIndex,
                unloadAllEmbeddedSceneObjects
                    ? UnloadSceneOptions.UnloadAllEmbeddedSceneObjects
                    : UnloadSceneOptions.None);
        }

        private static IEnumerator SetActiveScene(Scene scene)
        {
            yield return null;
            SceneManager.SetActiveScene(scene);
        }

        private void OnValidate()
        {
            isLoaded = IsLoaded;
            var worlds = Resources.FindObjectsOfTypeAll<HiraWorld>();
            foreach (var world in worlds)
            {
                if (this == world ^ world.buildIndex == buildIndex)
                    Debug.LogErrorFormat($"HiraWorld \"{name}\" has the same " +
                                         $"build index as HiraWorld \"{world.name}\".");
            }
        }
    }
}