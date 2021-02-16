using System.Collections.Generic;
 using UnityEngine;
using Object = UnityEngine.Object;

namespace HiraEngine.PoolTool
{
    internal class Pool : IPool
    {
        public Pool(Component bucket, Component target) => (_bucket, _target) = (bucket.transform, target);

        // destroy all pooled objects and clear the references
        public void Dispose()
        {
            foreach (var pooledObject in _pooledObjects) 
                Object.Destroy(pooledObject.gameObject);

            PoolCount = 0;
            _pooledObjects.Clear();
        }

        private readonly Component _target;
        private readonly List<Component> _pooledObjects = new List<Component>();
        private readonly Transform _bucket;
        public int PoolCount { get; private set; } = 0;

        #region Pool Access
        
        // get an object from the pool
        public Component GetObject()
        {
            PoolCount--;
            var pooledObject = _pooledObjects[PoolCount];
            _pooledObjects.RemoveAt(PoolCount);
            return pooledObject;
        }

        // return an object to the pool
        public void ReturnObject(Component component)
        {
            PoolCount++;
            component.transform.SetParent(_bucket);
            component.gameObject.SetActive(false);
            _pooledObjects.Add(component);
        }
        
        #endregion
        
        #region Pool modification

        // add a select number of instances to the pool
        public void AddToPool(byte count) { for (; count > 0; count--) AddToPool(); }

        // remove a select number of instances from the pool
        public void RemoveFromPool(byte count) { for (; count > 0; count--) RemoveFromPool(); }

        // add one instance to the pool
        public void AddToPool() => ReturnObject(Object.Instantiate(_target));
        // remove one instance from the pool
        public void RemoveFromPool() => Object.Destroy(GetObject().gameObject);
        
        #endregion
    }
}