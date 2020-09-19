using System.Collections.Generic;

namespace UnityEngine
{
    [CreateAssetMenu(menuName = "Hiralal/Pool Key", fileName = "New Pool Key")]
    public class PoolKey : ScriptableObject
    {
        private static readonly Dictionary<int, PoolToolAbstract> hash_map = new Dictionary<int, PoolToolAbstract>();

        private int? _ownerInstanceId = null;

        public void SetOwner(PoolToolAbstract poolTool)
        {
            if (_ownerInstanceId.HasValue)
            {
                var context = hash_map[_ownerInstanceId.Value].gameObject;
                Debug.LogErrorFormat(context, $"PoolKey {name} has already been claimed by the " +
                                          $"AddressablePoolTool on GameObject {context.name}.");
                return;
            }

            _ownerInstanceId = poolTool.GetInstanceID();
            hash_map.Add(_ownerInstanceId.Value, poolTool);
        }

        public void ClearOwner()
        {
            if (!_ownerInstanceId.HasValue)
            {
                Debug.LogErrorFormat(this, $"PoolKey {name} has not been claimed yet.");
                return;
            }

            hash_map.Remove(_ownerInstanceId.Value);
            _ownerInstanceId = null;
        }

        public PoolToolAbstract GetPool()
        {
            if (!_ownerInstanceId.HasValue)
            {
                Debug.LogErrorFormat(this, $"PoolKey {name} has not been claimed yet.");
                return null;
            }

            return hash_map[_ownerInstanceId.Value];
        }
    }
}