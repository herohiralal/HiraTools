/*
 * Name: HiraBlackboard.cs
 * Created By: Rohan Jadav
 * Description: A blackboard component, to get/set relevant values.
 */

using Hiralal.Blackboard;

namespace UnityEngine
{
    public class HiraBlackboard : MonoBehaviour
    {
        // TODO: Create a Readme for this.
        
        [SerializeField] private HiraBlackboardKeySet keySet = null;
        public IHiraMap MapComponent { get; private set; }

        private void Awake()
        {
            if (keySet != null) MapComponent = new HiraMap(keySet);
            else
            {
                Debug.LogErrorFormat(gameObject, $"A blackboard needs a key set to function.");
                Destroy(this);
            }
            
            Awake_Override();
        }

        private void OnDestroy()
        {
            OnDestroy_Override();
            MapComponent = null;
        }

        protected virtual void Awake_Override()
        {
        }

        protected virtual void OnDestroy_Override()
        {
        }
    }
}