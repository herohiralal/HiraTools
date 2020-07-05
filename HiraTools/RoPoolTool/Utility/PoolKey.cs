using System.Collections.Generic;
using HiraPoolTool.Abstractions;

namespace UnityEngine
{
    [CreateAssetMenu(menuName = "Hiralal/Pool Key", fileName = "New Pool Key")]
    public class PoolKey : ScriptableObject
    {
        private static readonly Dictionary<int, PoolToolAbstract> hash_map = new Dictionary<int, PoolToolAbstract>();

        private int? ownerInstanceId = null;

        public void SetOwner(PoolToolAbstract poolTool)
        {
            if (ownerInstanceId.HasValue)
            {
                var context = ((MonoBehaviour) hash_map[ownerInstanceId.Value]).gameObject;
                Debug.LogErrorFormat(context, $"PoolKey {name} has already been claimed by the " +
                                          $"AddressablePoolTool on GameObject {context.name}.");
                return;
            }

            ownerInstanceId = poolTool.GetInstanceID();
            hash_map.Add(ownerInstanceId.Value, poolTool);
        }

        public void ClearOwner()
        {
            if (!ownerInstanceId.HasValue)
            {
                Debug.LogErrorFormat(this, $"PoolKey {name} has not been claimed yet.");
                return;
            }

            hash_map.Remove(ownerInstanceId.Value);
            ownerInstanceId = null;
        }

        public PoolToolAbstract GetPool()
        {
            if (!ownerInstanceId.HasValue)
            {
                Debug.LogErrorFormat(this, $"PoolKey {name} has not been claimed yet.");
                return null;
            }

            return hash_map[ownerInstanceId.Value];
        }
    }
}